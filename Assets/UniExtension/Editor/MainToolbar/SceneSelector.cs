using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniExtension.Editor
{
    public class SceneSelector
    {
        private const string k_ElementPath = "Scene/Selector";
        private static string[] s_ScenePaths;

        [MainToolbarElement("Scene/Selector", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement Selector()
        {
            string activeSceneName;
            if (Application.isPlaying)
                activeSceneName = SceneManager.GetActiveScene().name;
            else
                activeSceneName = EditorSceneManager.GetActiveScene().name;
            if (activeSceneName.Length == 0)
                activeSceneName = "Untitled";

            var icon = EditorGUIUtility.IconContent("UnityLogo").image as Texture2D;
            var content = new MainToolbarContent(activeSceneName, icon, "Select active scene");
            return new MainToolbarDropdown(content, ShowDropdownMenu);
        }

        static void ShowDropdownMenu(Rect dropDownRect)
        {
            var menu = new GenericMenu();
            if (s_ScenePaths.Length == 0)
            {
                menu.AddDisabledItem(new GUIContent("No Scenes in Project"));
            }
            foreach (string scenePath in s_ScenePaths)
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
            s_ScenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
        }

        static void SceneSwitched(Scene oldScene, Scene newScene)
        {
            MainToolbar.Refresh(k_ElementPath);
        }

        static SceneSelector()
        {
            RefreshSceneList();
            EditorApplication.projectChanged += RefreshSceneList;
            SceneManager.activeSceneChanged += SceneSwitched;
            EditorSceneManager.activeSceneChangedInEditMode += SceneSwitched;
        }
    }
}
