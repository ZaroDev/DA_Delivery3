using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameAnalytics.Components
{
    [Serializable]
    public class PlayerDeaths
    {
        public readonly List<Vector3> Positions = new();
        public int Id;
        public int SessionId;
        public Color Color;
        public bool Show = true;
    }
    
    [ExecuteAlways]
    public class DeathDrawer : MonoBehaviour
    {
        public List<PlayerDeaths> Deaths = new();
        [SerializeField] private MeshRenderer meshRenderer;
        private Material _material;
        
        private float[] _grid;
        private int _count;
        private static readonly int UGrid = Shader.PropertyToID("_Grid");
        private static readonly int UCount = Shader.PropertyToID("_Count");

        public void Init()
        {
            _material = meshRenderer.sharedMaterial;
            _grid = new float[32 * 3];

            foreach (var p in Deaths.SelectMany(d => d.Positions))
            {
                AddHitPoint(p);
            }
        }

        private void AddHitPoint(Vector3 pos)
        {
            pos.y += 10f;
            Ray ray = new Ray(pos, transform.forward * 1);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("HeatMapLayer")))
            {
                _grid[_count * 3] = hit.textureCoord.x * 4 - 2;
                _grid[_count * 3 + 1] = hit.textureCoord.y * 4 - 2;
                _grid[_count * 3 + 2] += 1;
                
                _count++;
                _count %= 32;

                _material.SetFloatArray(UGrid, _grid);
                _material.SetInt(UCount, _count);
            }
        }
        
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
