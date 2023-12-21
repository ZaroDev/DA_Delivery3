using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace GameAnalytics.Data
{
    public class DataSender : MonoBehaviour
    {
        public static DataSender Instance { get; private set; }

        private static SessionData _currentSession;
        
        private enum PostType : int
        {
            SessionStart = 0,
            SessionEnd,
            PlayerAdd,
            PlayerHit,
            PlayerDie,
            PlayerKill
        }

        private static readonly Dictionary<PostType, string> Urls = new()
        {
            { PostType.PlayerAdd, "https://citmalumnes.upc.es/~victorfz/AddPlayer.php" },
            { PostType.SessionStart, "https://citmalumnes.upc.es/~victorfz/SendSessionStart.php" },
            { PostType.SessionEnd, "https://citmalumnes.upc.es/~victorfz/SendSessionEnd.php" },
            { PostType.PlayerHit, "https://citmalumnes.upc.es/~victorfz/SendPlayerHit.php"},
            { PostType.PlayerDie, "https://citmalumnes.upc.es/~victorfz/SendPlayerDead.php"},
            { PostType.PlayerKill, "https://citmalumnes.upc.es/~victorfz/SendPlayerKill.php"},
        };

        private void OnEnable()
        {
            PostSessionStart();
        }

        private void OnDisable()
        {
            PostSessionEnd();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
            Instance = this; 
        }

        public void PostPlayerHit(HitData data)
        {
            data.sessionId = _currentSession.id;
            PostData(Urls[PostType.PlayerHit], data);
        }

        public void PostPlayerKill(PositionData data)
        {
            data.sessionId = _currentSession.id;
            PostData(Urls[PostType.PlayerKill], data);
        }

        public void PostPlayerDie(PositionData data)
        {
            data.sessionId = _currentSession.id;
            PostData(Urls[PostType.PlayerDie], data);
        }
        
        private void PostSessionStart()
        {
            _currentSession = new SessionData(DateTime.Now);
            PostData(Urls[PostType.SessionStart], _currentSession);
        }

        private void PostSessionEnd()
        {
            _currentSession.SetEndTime(DateTime.Now);
            PostDataEditor(Urls[PostType.SessionEnd], _currentSession);
        }
        
        private void PostData(string url, Data data)
        {
            StartCoroutine(SendData(url, data));
        }
        
#if UNITY_EDITOR
        public static void PostPlayerData(PlayerData data)
        {
            PostDataEditor(Urls[PostType.PlayerAdd] ,data);
        }
        
        private static void PostDataEditor(string url, Data data)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(SendData(url, data));
        }
#endif
       
        
        private static IEnumerator SendData(string url, Data data)
        {
            // Serialize the data to JSON
            string jsonData = JsonUtility.ToJson(data);
            Debug.Log($"Sending: {jsonData}");
            // Create a UnityWebRequest to send a POST request
            var request = new UnityWebRequest(url, "POST");
            // Encode the data into bytes 
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            Debug.Log("Sending server request ...");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error sending the request: {request.error}");
            }
            else
            {
                var response = request.downloadHandler.text;
                request.Dispose();
                Debug.Log($"Data sent to server successfully PHP response: {response}");

                var serverResponse = JsonUtility.FromJson<Data>(response);
            
                // Wait for unity to dispose everything
                yield return new WaitForEndOfFrame();
                data.OnCreate(serverResponse.id);
            }
        }
    }
}

