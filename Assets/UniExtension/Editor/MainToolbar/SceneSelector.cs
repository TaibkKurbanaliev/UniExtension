using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
#if UNITY_6000_3_OR_NEWER
using UnityEditor.Toolbars;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UniExtension.Editor
{
    [InitializeOnLoad]
    public class SceneSelector
    {
        private const string k_ElementPath = "Scene/Selector";
        private static string[] _scenePaths;

#if UNITY_6000_3_OR_NEWER
        [MainToolbarElement(k_ElementPath, defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement Selector()
        {
            GetToolbarData(out var text, out var icon);
            var content = new MainToolbarContent(text, icon, "Select active scene");
            return new MainToolbarDropdown(content, ShowDropdownMenu);
        }
#endif

        static void GetToolbarData(out string sceneName, out Texture2D icon)
        {
            sceneName = Application.isPlaying
                ? SceneManager.GetActiveScene().name
                : EditorSceneManager.GetActiveScene().name;

            if (string.IsNullOrEmpty(sceneName))
                sceneName = "Untitled";

            icon = EditorGUIUtility.IconContent("UnityLogo").image as Texture2D;
        }

        static void ShowDropdownMenu(Rect dropDownRect)
        {
            var menu = new GenericMenu();
            if (_scenePaths.Length == 0)
            {
                menu.AddDisabledItem(new GUIContent("No Scenes in Project"));
            }
            foreach (string scenePath in _scenePaths)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                menu.AddItem(new GUIContent(sceneName), false, () =>
                {
                    SwitchScene(scenePath);
                });
            }
            menu.DropDown(dropDownRect);
        }

        static void SwitchScene(string scenePath)
        {
            if (Application.isPlaying)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (Application.CanStreamedLevelBeLoaded(sceneName))
                {
                    Debug.Log($"Switching to scene: {sceneName}");
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogError($"Scene '{sceneName}' is not in the Build Settings.");
                }
            }
            else
            {
                if (File.Exists(scenePath))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        Debug.Log($"Switching to scene: {scenePath}");
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
                else
                {
                    Debug.LogError($"Scene at path '{scenePath}' does not exist.");
                }
            }
        }

        static void RefreshSceneList()
        {
            _scenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
        }

        static void SceneSwitched(Scene oldScene, Scene newScene)
        {
#if UNITY_6000_3_OR_NEWER
            MainToolbar.Refresh(k_ElementPath);
#endif
        }
#if UNITY_6000_3_OR_NEWER
        static SceneSelector()
        {
            RefreshSceneList();
            EditorApplication.projectChanged += RefreshSceneList;
            SceneManager.activeSceneChanged += SceneSwitched;
            EditorSceneManager.activeSceneChangedInEditMode += SceneSwitched;
        }
#endif

#if !UNITY_6000_3_OR_NEWER
        static IMGUIContainer _legacyContainer;

        static SceneSelector()
        {
            RefreshSceneList();

            EditorApplication.projectChanged += RefreshSceneList;
            SceneManager.activeSceneChanged += SceneSwitched;
            EditorSceneManager.activeSceneChangedInEditMode += SceneSwitched;

            EditorApplication.delayCall += TryAttachToLegacyToolbar;
        }

        static void TryAttachToLegacyToolbar()
        {
            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
            var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);

            if (toolbars.Length == 0)
            {
                EditorApplication.delayCall += TryAttachToLegacyToolbar;
                return;
            }

            var toolbar = toolbars[0];
            var root = toolbarType
                .GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(toolbar) as VisualElement;

            if (root == null)
                return;

            // Центральная зона (кнопки Play)
            var centerZone = root.Q("ToolbarZonePlayMode");
            if (centerZone == null)
                return;

            _legacyContainer = new IMGUIContainer(DrawLegacyToolbarGUI)
            {
                style =
                {
                    marginLeft = 6,
                    marginRight = 6
                }
            };

            centerZone.Add(_legacyContainer);
        }

        static void DrawLegacyToolbarGUI()
        {
            GetToolbarData(out var text, out _);

            if (GUILayout.Button(text, EditorStyles.popup, GUILayout.Width(160)))
            {
                var rect = GUILayoutUtility.GetLastRect();
                rect.y += rect.height;
                ShowDropdownMenu(rect);
            }
        }
#endif
    }
}
