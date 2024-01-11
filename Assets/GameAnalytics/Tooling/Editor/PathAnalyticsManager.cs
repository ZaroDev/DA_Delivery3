using System;
using System.Collections.Generic;
using System.Linq;
using GameAnalytics.Components;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameAnalytics.Tooling.Editor
{
    [Serializable]
    public class PositionJSON
    {
        public int session_id;
        public int player_id;
        public float x;
        public float y;
        public float z;
    }
    [Serializable]
    public class PathJSON
    {
        public PositionJSON[] positions;
    }
    public static class PathAnalyticsManager
    {
        public static List<Path> Paths = new List<Path>();
        private static PathDrawer _container;

        public static void ClearPaths()
        {
            Object.DestroyImmediate(_container.gameObject);
            Paths.Clear();
        }
        
        private static void GenerateAllPaths(PathJSON path)
        {
            var groupedPositions = path.positions.GroupBy(pos => (pos.session_id, pos.player_id)).ToList();
            
            foreach (var group  in groupedPositions)
            {
                var p = new Path();
                p.Color = Random.ColorHSV();
                p.PlayerId = group.Key.player_id;
                p.SessionId = group.Key.session_id;

                p.Positions = group.ToArray().Select(json => new Vector3(json.x, json.y, json.z)).ToArray();
                
                Paths.Add(p);
            }

            var go = new GameObject("Path Container");
            _container = go.AddComponent<PathDrawer>();
            _container.paths = Paths;
        }
        
        public static void GetPathData()
        {
            DataGetter.GetPaths(ProcessPathJson);
        }

        private static void ProcessPathJson(string jsonString)
        {
            var path = JsonUtility.FromJson<PathJSON>(jsonString);
            
            GenerateAllPaths(path);
        }
    }
}