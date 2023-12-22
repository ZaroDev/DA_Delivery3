using UnityEditor;
using UnityEngine;

namespace GameAnalytics.Tooling
{
    public class GameAnalyticsWindow : EditorWindow
    {
        private GameObject pathArrowPrefab;
        private GameObject startPathPrefab;
        private GameObject endPathPrefab;

        [MenuItem("Game Analytics/Panel")]
        public static void ShowWindow()
        {
            GetWindow<GameAnalyticsWindow>("Game Analytics");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Arrow");
            pathArrowPrefab = (GameObject)EditorGUILayout.ObjectField(pathArrowPrefab, typeof(GameObject), false);
            EditorGUILayout.LabelField("Start");
            startPathPrefab = (GameObject)EditorGUILayout.ObjectField(startPathPrefab, typeof(GameObject), false);
            EditorGUILayout.LabelField("End");
            endPathPrefab = (GameObject)EditorGUILayout.ObjectField(endPathPrefab, typeof(GameObject), false);

            if(GUILayout.Button("Generate paths"))
            {
                PathAnalyticsManager.GetPathData(pathArrowPrefab, startPathPrefab, endPathPrefab);
            }
        }
    
    }
}
