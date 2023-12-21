using GameAnalytics.Data;
using Gamekit3D;
using Gamekit3D.Message;
using UnityEngine;

namespace GameAnalytics.Runtime
{
    public class GameAnalyticsEventManager : MonoBehaviour, IMessageReceiver
    {
        private void OnEnable()
        {
            SubscribeToDamageEvents();
            SubscribeToDeadVolumeEvents();
        }

        private void SubscribeToDamageEvents()
        {
            var damageables = FindObjectsByType<Damageable>(FindObjectsSortMode.None);
            foreach (var damagable in damageables)
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
                    ProcessDamageEvent((Damageable.DamageMessage)msg, (Damageable)sender);
                    break;
                case MessageType.DEAD:
                    ProcessDeadEvent((Damageable.DamageMessage)msg, (Damageable)sender);
                    break;
                case MessageType.RESPAWN:
                    break;
            }
        }

        private void ProcessDamageEvent(Damageable.DamageMessage msg, Damageable sender)
        {
            var tracker = sender.GetComponent<EntityTracker>();
            var data = new HitData((uint)tracker.id, sender.transform.position, msg.amount, sender.currentHitPoints);
            DataSender.Instance.PostPlayerHit(data);
        }
        private void ProcessDeadEvent(Damageable.DamageMessage msg, Damageable sender)
        {
            // Death event
            {
                var tracker = sender.GetComponent<EntityTracker>();
                var data = new PositionData((uint)tracker.id, sender.transform.position);
                DataSender.Instance.PostPlayerDie(data);
            }
            // Kill event
            {
                var tracker = msg.damager.GetComponent<EntityTracker>();
                // Check if the instigator is a child
                if (tracker == null)
                {
                    tracker = msg.damager.GetComponentInParent<EntityTracker>();
                }

                if (tracker == null)
                {
                    return;
                }
                
                var data = new PositionData((uint)tracker.id, msg.damageSource);
                DataSender.Instance.PostPlayerKill(data);
            }
        }
    }
}
