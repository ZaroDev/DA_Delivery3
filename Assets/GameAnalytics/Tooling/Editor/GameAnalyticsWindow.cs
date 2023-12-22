using UnityEditor;
using UnityEngine;

namespace GameAnalytics.Tooling
{
    public class GameAnalyticsWindow : EditorWindow
    {
        [MenuItem("Game Analytics/Panel")]
        public static void ShowWindow()
        {
            GetWindow<GameAnalyticsWindow>("Game Analytics");
        }
        
        private void OnGUI()
        {
            //GUILayout.
        }
    
    }
}
