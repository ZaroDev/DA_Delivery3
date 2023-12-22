using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalytics.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameAnalytics
{
    [ExecuteAlways]
    public class EntityTracker : MonoBehaviour
    {
        public uint id;

        [SerializeField, Tooltip("Time interval in seconds for sending the position data to the server")] 
        private float trackInterval = 1f;


        private Coroutine _coroutine;
        public void Start()
        {
            _coroutine = StartCoroutine(SendPosition());
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
#if UNITY_EDITOR
        public void RequestID()
        {
            var playerData = new PlayerData(0, gameObject.name);
            playerData.callback += (uint newId) =>
            {
                this.id = newId;
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            };

            DataSender.PostPlayerData(playerData);
        }
#endif
        private IEnumerator SendPosition()
        {
            var startPos = transform.position;
            var sendFirstPos = true;
            while (true)
            {
                if (transform.position != startPos || sendFirstPos)
                {
                    var data = new PositionData(id, transform.position);
                    sendFirstPos = false;
                    if (DataSender.Instance != null)
                    {
                        DataSender.Instance.PostPlayerPosition(data);
                    }
                }
                yield return new WaitForSeconds(trackInterval);
            }
        }
    }
}
