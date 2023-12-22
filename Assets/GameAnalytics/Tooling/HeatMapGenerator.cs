using UnityEngine;

namespace GameAnalytics.Tooling
{
    public class HeatMapGenerator : MonoBehaviour
    {
        private HeatMap _heatMap;

        private void Start()
        {
            FillHeatmap();
            DrawHeatmap();
        }

        private void FillHeatmap()
        {
            float[,,] values = new float[10,5,10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        values[x, y, z] = Random.Range(0f, 1f);
                    }
                }
            }

            _heatMap = new HeatMap(10, 5, 10,1f, values);
        }

        private void DrawHeatmap()
        {
            for(int x = 0; x < _heatMap.SizeX; x++)
            {
                for (int y = 0; y < _heatMap.SizeY; y++)
                {
                    for (int z = 0; z < _heatMap.SizeZ; z++)
                    {
                        Vector3 pos = new Vector3(x, y, z) * _heatMap.Spacing;
                        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        go.transform.position = pos;
                        go.transform.rotation = Quaternion.identity;

                        Color col = Color.Lerp(_heatMap.MinColor, _heatMap.MaxColor, _heatMap.Values[x, y, z]);
                        Debug.Log($"{_heatMap.Values[x, y, z]}");
                        go.GetComponent<Renderer>().material.color = col;
                    }
                }
            }
        }
    }
}
