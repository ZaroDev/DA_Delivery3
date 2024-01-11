using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
#if UNITY_EDITOR
        public List<Path> Paths = new();
        private void OnDrawGizmos()
        {
            foreach (var path in Paths.Where(path => path.Show))
            {
                Handles.color = path.Color;
                Gizmos.DrawIcon(path.Positions.First(), "GameAnalytics/StartGizmo.png", true, path.Color);
                Handles.DrawAAPolyLine(path.Positions);
                Gizmos.DrawIcon(path.Positions.Last(), "GameAnalytics/EndGizmo.png", true, path.Color);
            }
        }
#endif
    }
}
