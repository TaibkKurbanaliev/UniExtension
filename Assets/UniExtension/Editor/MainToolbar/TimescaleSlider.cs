using System.Reflection;
using UnityEditor;
#if UNITY_6000_3_OR_NEWER
using UnityEditor.Toolbars;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace UniExtension.Editor
{
    [InitializeOnLoad]
    public class TimescaleSlider
    {
        const float k_MinTimescale = 0f;
        const float k_MaxTimescale = 5f;
        const float k_Padding = 10f;
#if UNITY_6000_3_OR_NEWER
        [MainToolbarElement("Timescale/Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeSlider()
        {
            var content = new MainToolbarContent("Timescale", "Timescale");
            var slider = new MainToolbarSlider(content, Time.timeScale, k_MinTimescale,
                                               k_MaxTimescale, OnSliderValueChanged);

            return slider;
        }

        [MainToolbarElement("Timescale/Reset", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeResetButton()
        {
            var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
            var content = new MainToolbarContent(icon, "Reset");
            var button = new MainToolbarButton(content, () => {
                Time.timeScale = 1f;
                MainToolbar.Refresh("Timescale/Slider");
            });

            return button;
        }

        private static void OnSliderValueChanged(float value)
        {
            Time.timeScale = value;
        }
#endif
#if !UNITY_6000_3_OR_NEWER
        static IMGUIContainer _sliderContainer;

        static TimescaleSlider()
        {
            EditorApplication.delayCall += TryAttachToLegacyToolbar;
        }

        static void TryAttachToLegacyToolbar()
        {
            var toolbarType = typeof(UnityEditor.Editor)
                .Assembly
                .GetType("UnityEditor.Toolbar");

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

            var centerZone = root.Q("ToolbarZonePlayMode");
            if (centerZone == null)
                return;

            _sliderContainer = new IMGUIContainer(DrawLegacyGUI)
            {
                style =
                {
                    marginLeft = 6,
                    marginRight = 6
                }
            };

            centerZone.Add(_sliderContainer);
        }

        static void DrawLegacyGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(150));

            EditorGUI.BeginChangeCheck();
            float value = GUILayout.HorizontalSlider(
                Time.timeScale,
                k_MinTimescale,
                k_MaxTimescale,
                GUILayout.Width(120)
            );

            if (EditorGUI.EndChangeCheck())
                Time.timeScale = value;

            GUILayout.Label(Time.timeScale.ToString("0.00"),
                            EditorStyles.miniLabel,
                            GUILayout.Width(32));

            if (GUILayout.Button("↩", GUILayout.Width(24)))
                Time.timeScale = 1f;

            GUILayout.EndHorizontal();
        }
#endif
    }
}
