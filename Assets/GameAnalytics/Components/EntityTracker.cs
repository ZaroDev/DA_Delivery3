using GameAnalytics.Data;
using UnityEditor;
using UnityEngine;

namespace GameAnalytics
{
    [ExecuteAlways]
    public class EntityTracker : MonoBehaviour
    {
        [ShowOnly] public int id;
        public void RequestID()
        {
            var playerData = new PlayerData(0, gameObject.name);
            playerData.callback += (uint newId) =>
            {
                this.id = (int)newId;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            };

            DataSender.PostPlayerData(playerData);
        }    
    }
}
