using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D
{
    [RequireComponent(typeof(Collider))]
    public class DeathVolume : MonoBehaviour
    {
        public new AudioSource audio;

        [EnforceType(typeof(Message.IMessageReceiver))]
        public List<MonoBehaviour> onDamageMessageReceivers;

        void OnTriggerEnter(Collider other)
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                if(!pc.respawning)
                {
                    SendMessage();
                }
                pc.Die(new Damageable.DamageMessage());
            }
            if (audio != null)
            {
                audio.transform.position = other.transform.position;
                if (!audio.isPlaying)
                    audio.Play();
            }
        }

        private void SendMessage()
        {
            for (var i = 0; i < onDamageMessageReceivers.Count; ++i)
            {
                var receiver = onDamageMessageReceivers[i] as IMessageReceiver;

                Damageable.DamageMessage message = new Damageable.DamageMessage();
                message.damager = this;
                message.damageSource = transform.position;
                
                receiver.OnReceiveMessage(MessageType.DEAD, this, message);
            }
        }

        void Reset()
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Default")
                gameObject.layer = LayerMask.NameToLayer("Environment");
            var c = GetComponent<Collider>();
            if (c != null)
                c.isTrigger = true;
        }

    }
}
