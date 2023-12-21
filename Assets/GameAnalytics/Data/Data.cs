using System;
using UnityEngine;

namespace GameAnalytics.Data
{
    [System.Serializable]
    public class Data
    {
        public uint id;
        public Action<uint> callback;
        public virtual void OnCreate(uint id = 0) { }
    }

    [System.Serializable]
    public class PlayerData : Data
    {
        public string name;
        public PlayerData(uint id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override void OnCreate(uint id = 0)
        {
            this.id = id;
            callback?.Invoke(id);
        }
    }


    [System.Serializable]
    public class SessionData : Data
    {
        public string startTime;
        public string endTime;

        private bool _end = false;
        public SessionData(DateTime time)
        {
            this.startTime = time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void SetEndTime(DateTime time) => endTime = time.ToString("yyyy-MM-dd HH:mm:ss");
        public override void OnCreate(uint id = 0)
        {
            this.id = id;
        }
    }

    [System.Serializable]
    public class PositionData : Data 
    {
        public uint sessionId;
        public float x, y, z;
        public string time;
        public PositionData(uint id, Vector3 position)
        {
            this.id = id;
            this.x = position.x;
            this.y = position.y;
            this.z = position.z;
            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    [System.Serializable]
    public class HitData : PositionData
    {
        public float damage;
        public float health;
        
        public HitData(uint id, Vector3 position, float damage, float health)
            : base(id, position)
        {
            this.damage = damage;
            this.health = health;
        }
    }
}