using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace UniExtension.Editor
{
    public class TimescaleSlider
    {
        const float k_MinTimescale = 0f;
        const float k_MaxTimescale = 5f;
        const float k_Padding = 10f;

        [MainToolbarElement("Timescale/Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeSlider()
        {
            var content = new MainToolbarContent("Timescale", "Timescale");
            var slider = new MainToolbarSlider(content, Time.timeScale,k_MinTimescale, 
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
    }
}
