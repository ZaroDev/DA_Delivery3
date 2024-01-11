using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;
using Unity.Mathematics;
using UnityEditor;

namespace GameAnalytics.Components
{
    public class Path
    {
        public Vector3[] Positions;
        public Color Color;
        public int PlayerId;
        public int SessionId;
        public bool Show = true;
    }
    
    [ExecuteAlways]
    public class PathDrawer : MonoBehaviour
    {
        public List<Path> paths = new();
        private void OnDrawGizmos()
        {
            foreach (var path in paths.Where(path => path.Show))
            {
                Gizmos.color = path.Color;
                Gizmos.DrawLineList(path.Positions);
            }
        }

    }
}
