#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace TimToolBox.Editor
{
    public class TimeScaleEditorWindow
        : EditorWindow

    {
        private float timeScale = 1.0f;

        // Add menu item to open the window
        [MenuItem("Tim'sToolBox/Time Scale Controller")]
        public static void ShowWindow()
        {
            GetWindow<TimeScaleEditorWindow>("Time Scale Controller");
        }

        private void OnGUI()
        {
            GUILayout.Label("Time Scale Controller", EditorStyles.boldLabel);

            // Display a slider for timescale adjustment
            timeScale = EditorGUILayout.Slider("Time Scale", timeScale, 0f, 5f);

            // Update Time.timeScale with the new value
            if (Mathf.Abs(Time.timeScale - timeScale) > 0.01f) // only update if there's a notable difference
            {
                Time.timeScale = timeScale;
            }

            // Reset button to quickly reset time scale to normal
            if (GUILayout.Button("Reset Time Scale"))
            {
                timeScale = 1.0f;
                Time.timeScale = 1.0f;
            }

            // Display the current timescale
            EditorGUILayout.LabelField("Current Time Scale:", Time.timeScale.ToString("F2"));
        }

        private void OnDisable()
        {
            // Reset timescale to 1 when the window is closed
            Time.timeScale = 1.0f;
        }
    }
}
#endif