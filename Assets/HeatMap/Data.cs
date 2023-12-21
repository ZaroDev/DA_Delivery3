using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    public SessionData(uint id, DateTime time)
    {
        this.id = id;
        this.startTime = time.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void SetEndTime(DateTime time) => endTime = time.ToString("yyyy-MM-dd HH:mm:ss");
    public override void OnCreate(uint id = 0)
    {
       
    }
}

[System.Serializable]
public class PositionData : Data 
{
    public float x, y, z; 
    public PositionData(uint id, Vector3 postition)
    {
        this.id = id;
        this.x = postition.x;
        this.y = postition.y;
        this.z = postition.z;
    }
}

[System.Serializable]
public class HitData : PositionData
{
    public float damage;
    public HitData(uint id, Vector3 postition, float damage)
        : base(id, postition)
    {
        this.damage = damage;
    }
}