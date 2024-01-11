using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameAnalytics.Components
{
    public class PlayerDeaths
    {
        public List<Vector3> Positions = new();
        public int Id;
        public Color Color;
        public bool Show = true;
    }
    
    [ExecuteAlways]
    public class DeathDrawer : MonoBehaviour
    {
        public readonly List<PlayerDeaths> Deaths = new List<PlayerDeaths>();
        
        private void OnDrawGizmos()
        {
            foreach (var death in Deaths)
            {
                if(!death.Show)
                { 
                    continue;
                }
                
                foreach (var pos in death.Positions)
                {
                    Gizmos.DrawIcon(new Vector3(pos.x, pos.y + 1f, pos.z), "GameAnalytics/SkullGizmo.png", true, death.Color);
                }
            }
        }
    }
}
