using System.Collections.Generic;
using System.Linq;
using GameAnalytics.Components;
using UnityEngine;

namespace GameAnalytics.Tooling.Editor
{
    [System.Serializable]
    public class DeathJSONData
    {
        public int player_id;
        public float x;
        public float y;
        public float z;
    }
    [System.Serializable]
    public class DeathJSONs
    {
        public DeathJSONData[] deaths;
    }
    public static class DeathAnalyticsManager
    {
        public static DeathDrawer Drawer;

        public static void ClearDeaths()
        {
            Object.DestroyImmediate(Drawer.gameObject);
        }
        
        public static void GatherAllData()
        {
            DataGetter.GetDeaths(ProcessDeathJson);
        }
        private static void ProcessDeathJson(string jsonString)
        {
            var deaths = JsonUtility.FromJson<DeathJSONs>(jsonString);
            
            var groupedDeaths = deaths.deaths.GroupBy(death => death.player_id);
            
            var go = new GameObject("Death Drawer");
            Drawer = go.AddComponent<DeathDrawer>();
            
            foreach (var death in groupedDeaths)
            {
                var d = new PlayerDeaths
                {
                    Color = Random.ColorHSV(),
                    Id = death.Key
                };
                var deathData = death.ToList();
                foreach (var deathJsonData in deathData)
                {
                    d.Positions.Add(new Vector3(deathJsonData.x, deathJsonData.y, deathJsonData.z));
                }
                Drawer.Deaths.Add(d);
            }
        }
    }
}
