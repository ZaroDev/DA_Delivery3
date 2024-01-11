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
        public int session_id;
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
        public static GameObject DeathDrawerGO;

        public static void ClearDeaths()
        {
            if (DeathDrawerGO != null)
            {
                Drawer.Deaths.Clear();
                Object.DestroyImmediate(DeathDrawerGO);
            }
        }
        
        public static void GatherAllData()
        {
            DataGetter.GetDeaths(ProcessDeathJson);
        }
        private static void ProcessDeathJson(string jsonString)
        {
            var deaths = JsonUtility.FromJson<DeathJSONs>(jsonString);
            
            var groupedDeaths = deaths.deaths.GroupBy(death => death.player_id);
            if (Drawer == null)
            {
                var go = GameObject.Instantiate(DeathDrawerGO);
                Drawer = go.GetComponent<DeathDrawer>();
            }

            foreach (var death in groupedDeaths)
            {
                var d = new PlayerDeaths
                {
                    Color = Random.ColorHSV(),
                    Id = death.Key,
                };
                var deathData = death.ToList();
                foreach (var deathJsonData in deathData)
                {
                    d.Positions.Add(new Vector3(deathJsonData.x, deathJsonData.y, deathJsonData.z));
                }
                Drawer.Deaths.Add(d);
            }

            GameAnalyticsWindow.AllDeaths = Drawer.Deaths;
            Drawer.Init();
        }
    }
}
