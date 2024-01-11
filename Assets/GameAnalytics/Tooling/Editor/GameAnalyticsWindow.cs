using UnityEditor;
using UnityEngine;

namespace GameAnalytics.Tooling.Editor
{
    public class GameAnalyticsWindow : EditorWindow
    {
        private static bool _showDeaths = false;
        private static bool _showPaths = false;
        private static Vector2 _scrollPos = new();
        [MenuItem("Game Analytics/Panel")]
        public static void ShowWindow()
        {
            GetWindow<GameAnalyticsWindow>("Game Analytics");
        }
        
        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            
            EditorGUILayout.LabelField("Paths");
            
            if(GUILayout.Button("Gather and generate paths"))
            {
                PathAnalyticsManager.GetPathData();
            }
            
            if (GUILayout.Button("Reset"))
            {
                PathAnalyticsManager.ClearPaths();
            }

            if (PathAnalyticsManager.Paths.Count != 0)
            {
                _showPaths = EditorGUILayout.BeginFoldoutHeaderGroup(_showPaths, "Paths");
                if (_showPaths)
                {
                    foreach (var p in PathAnalyticsManager.Paths)
                    {
                        EditorGUILayout.LabelField($"Player id {p.PlayerId}, session id {p.SessionId}");
                        p.Color = EditorGUILayout.ColorField(p.Color);
                        p.Show = EditorGUILayout.Toggle("Show", p.Show);
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("Deaths");

            if (GUILayout.Button("Gather and generate deaths"))
            {
                DeathAnalyticsManager.GatherAllData();
            }

            if (GUILayout.Button("Clear deaths"))
            {
                DeathAnalyticsManager.ClearDeaths();
            }
            
            if (DeathAnalyticsManager.Drawer != null)
            {
                _showDeaths = EditorGUILayout.BeginFoldoutHeaderGroup(_showDeaths, "Deaths");
                if (_showDeaths)
                {
                    foreach (var death in DeathAnalyticsManager.Drawer.Deaths)
                    {
                        EditorGUILayout.LabelField($"Player: {death.Id}");
                        death.Color = EditorGUILayout.ColorField(death.Color);
                        death.Show = EditorGUILayout.Toggle("Show", death.Show);
                        EditorGUILayout.Separator();
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            EditorGUILayout.EndScrollView();
        }
    
    }
}
