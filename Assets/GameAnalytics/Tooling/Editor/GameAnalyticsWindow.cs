using System.Collections.Generic;
using System.Linq;
using GameAnalytics.Components;
using UnityEditor;
using UnityEngine;

namespace GameAnalytics.Tooling.Editor
{
    public class GameAnalyticsWindow : EditorWindow
    {
        private static bool _showDeaths = false;
        private static bool _showPaths = false;
        private static int _session = -1;
        private static int _player = -1;
        private static Vector2 _scrollPos = new();

        private static GameObject _deathDrawerGO;

        public static List<Path> AllPaths = new();
        public static List<PlayerDeaths> AllDeaths = new();
        
        [MenuItem("Game Analytics/Panel")]
        public static void ShowWindow()
        {
            GetWindow<GameAnalyticsWindow>("Game Analytics");
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (PathAnalyticsManager.Drawer != null && DeathAnalyticsManager.Drawer != null)
            {
                EditorGUILayout.LabelField("Filters");

                EditorGUILayout.LabelField("Session Id");

                var newSession = EditorGUILayout.IntField(_session);
                if (_session != newSession)
                {
                    _session = newSession;

                    if (_session != -1)
                    {
                        PathAnalyticsManager.Drawer.Paths =
                            AllPaths.Where(x => x.SessionId == _session).ToList();
                        
                        if(_player != -1)
                        {
                            PathAnalyticsManager.Drawer.Paths =
                                AllPaths.Where(x => x.PlayerId == _player && x.SessionId == _session).ToList();
                        }
                    }
                    else if (_session == -1)
                    {
                        PathAnalyticsManager.GetPathData();
                        DeathAnalyticsManager.GatherAllData();
                    }
                }

                EditorGUILayout.LabelField("Player Id");

                var newPlayer = EditorGUILayout.IntField(_player);
                if (_player != newPlayer)
                {
                    _player = newPlayer;
                    if (_player != -1)
                    {
                        PathAnalyticsManager.Drawer.Paths =
                            AllPaths.Where(x => x.PlayerId == _player).ToList();
                        DeathAnalyticsManager.Drawer.Deaths =
                            AllDeaths.Where(x => x.Id == _player).ToList();
                        
                        if (_session != -1)
                        {
                            PathAnalyticsManager.Drawer.Paths =
                                AllPaths.Where(x => x.PlayerId == _player && x.SessionId == _session).ToList();
                        }
                        DeathAnalyticsManager.Drawer.Init();
                    }
                    else if (_player == -1)
                    {
                        PathAnalyticsManager.GetPathData();
                        DeathAnalyticsManager.GatherAllData();
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Please generate all path and death data to start filters");
            }

            EditorGUILayout.LabelField("Paths");

            if (GUILayout.Button("Gather and generate paths"))
            {
                PathAnalyticsManager.GetPathData();
            }

            if (GUILayout.Button("Reset"))
            {
                PathAnalyticsManager.ClearPaths();
            }

            if (PathAnalyticsManager.Drawer != null)
            {
                if (PathAnalyticsManager.Drawer.Paths.Count != 0)
                {
                    EditorGUILayout.LabelField($"Path count: {PathAnalyticsManager.Drawer.Paths.Count}");
                    _showPaths = EditorGUILayout.BeginFoldoutHeaderGroup(_showPaths, "Paths");
                    if (_showPaths)
                    {
                        foreach (var p in PathAnalyticsManager.Drawer.Paths)
                        {
                            EditorGUILayout.LabelField($"Player id {p.PlayerId}, session id {p.SessionId}");
                            p.Color = EditorGUILayout.ColorField(p.Color);
                            p.Show = EditorGUILayout.Toggle("Show", p.Show);
                        }
                    }

                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
            }

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Deaths");

            EditorGUILayout.LabelField("Death drawer prefab");
            _deathDrawerGO = (GameObject)EditorGUILayout.ObjectField(_deathDrawerGO, typeof(GameObject), false);

            if (GUILayout.Button("Gather and generate deaths"))
            {
                DeathAnalyticsManager.DeathDrawerGO = _deathDrawerGO;
                DeathAnalyticsManager.GatherAllData();
            }

            if (GUILayout.Button("Clear deaths"))
            {
                DeathAnalyticsManager.ClearDeaths();
            }

            if (DeathAnalyticsManager.Drawer != null)
            {
                EditorGUILayout.LabelField($"Death count {DeathAnalyticsManager.Drawer.Deaths.Count}");
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