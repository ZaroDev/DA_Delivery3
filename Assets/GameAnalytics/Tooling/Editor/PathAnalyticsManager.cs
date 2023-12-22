using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAnalytics.Tooling
{
    [System.Serializable]
    public class Position
    {
        public int path_id;
        public int session_id;
        public int player_id;
        public float x;
        public float y;
        public float z;

        public string time;
        public Vector3 position;

        public void Init()
        {
            position = new Vector3(x,y,z);
        }
    }
    [System.Serializable]
    public class Path
    {
        public Position[] positions;
    }
    public static class PathAnalyticsManager
    {
        private static GameObject Arrow;
        private static GameObject Start;
        private static GameObject End;

        private static List<Path> Paths = new List<Path>();

        public static void GenerateAllPaths(Path path)
        {
            var groupedPositions = path.positions.GroupBy(pos => (pos.session_id, pos.player_id)).ToList();
            foreach(var position in groupedPositions) 
            {
                Path p = new Path();
                p.positions = position.ToArray();
                Paths.Add(p);
            }


            foreach(var p in Paths)
            {
                for(var i = 0; i < p.positions.Length; i++)
                {
                    var pos = p.positions[i];
                    if(i == 0)
                    {
                        GameObject.Instantiate(Start, pos.position, Quaternion.identity);
                        continue;
                    }
                    else if(i == p.positions.Length - 1)
                    {
                        GameObject.Instantiate(End, pos.position, Quaternion.identity);
                        continue;
                    }


                    GameObject.Instantiate(Arrow, pos.position, Quaternion.identity);
                }
            }
        }

        public static void GetPathData(GameObject arrow, GameObject start, GameObject end)
        {
            Arrow = arrow;
            Start = start;
            End = end;
            DataGetter.GetPaths(ProcessPathJSON);
        }

        public static void ProcessPathJSON(string jsonString)
        {
            Debug.Log(jsonString);
            var path = JsonUtility.FromJson<Path>(jsonString);
            foreach(var position in path.positions) 
            {
                position.Init();
            }
            GenerateAllPaths(path);
        }
    }
}