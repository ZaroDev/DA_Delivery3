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
        public static PathDrawer Drawer;

        public static void ClearPaths()
        {
            if (Drawer != null)
            {
                Object.DestroyImmediate(Drawer.gameObject);
            }
            Drawer.Paths.Clear();
        }
        
        private static void GenerateAllPaths(PathJSON path)
        {
            if (Drawer == null)
            {
                var go = new GameObject("Path Container");
                Drawer = go.AddComponent<PathDrawer>();
            }
            
            var groupedPositions = path.positions.GroupBy(pos => (pos.session_id, pos.player_id)).ToList();
            
            foreach (var group  in groupedPositions)
            {
                var p = new Path();
                p.Color = Random.ColorHSV();
                p.PlayerId = group.Key.player_id;
                p.SessionId = group.Key.session_id;
                p.Show = true;
                p.Positions = group.ToArray().Select(json => new Vector3(json.x, json.y + 1f, json.z)).ToArray();
                
                Drawer.Paths.Add(p);
            }

            GameAnalyticsWindow.AllPaths = Drawer.Paths;
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