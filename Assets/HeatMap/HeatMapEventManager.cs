using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct HeatMapEvents
{
    // Event for when a player dies
    static Action<Vector3> OnPlayerDied;
    // Event for when a player kills another
    static Action<Vector3> OnPlayerKill;
    // Event for when a player gets hit
    static Action<Vector3> OnPlayerHit;
}

public class HeatMapEventManager : Gamekit3D.Message.IMessageReceiver
{
    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        switch(type)
        {

        }
    }
}
