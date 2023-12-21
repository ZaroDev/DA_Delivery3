using Gamekit3D;
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

public class HeatMapEventManager : MonoBehaviour, Gamekit3D.Message.IMessageReceiver
{
    private void OnEnable()
    {
        SubscribeToDamageEvents();
        SubscribeToDeadVolumeEvents();
    }

    private void SubscribeToDamageEvents()
    {
        var damagables = FindObjectsByType<Damageable>(FindObjectsSortMode.None);
        foreach (var damagable in damagables)
        {
            damagable.onDamageMessageReceivers.Add(this);
        }
    }
    private void SubscribeToDeadVolumeEvents()
    {
        var volumes = FindObjectsByType<DeathVolume>(FindObjectsSortMode.None);
        foreach(var volume in volumes)
        {
            volume.onDamageMessageReceivers.Add(this);
        }
    }

    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        Debug.Log($"Message received {type}, {sender}, {msg}");
        
        switch (type)
        {
            case MessageType.DAMAGED:
                ProcessDamageEvent((Damageable.DamageMessage)msg, sender);
                break;
            case MessageType.DEAD:
                ProcessDeadEvent(sender);
                break;
            case MessageType.RESPAWN:
                break;
        }
    }

    private void ProcessDamageEvent(Damageable.DamageMessage msg, object sender)
    {
    }
    private void ProcessDeadEvent(object sender)
    {
    }
}
