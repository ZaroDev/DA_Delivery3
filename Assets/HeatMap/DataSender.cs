using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DataSender : MonoBehaviour
{
    public static DataSender Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this; 
        DontDestroyOnLoad(this);
    }


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
       { PostType.SessionStart, "https://citmalumnes.upc.es/~pabloldc/SendSessionStart.php" },
       { PostType.SessionEnd, "https://citmalumnes.upc.es/~pabloldc/SendSessionEnd.php" },
    };

    public void PostPlayerData(PlayerData data)
    {
        PostData(Urls[PostType.PlayerAdd] ,data);
    }

    private void PostData(string url, Data data)
    {
        StartCoroutine(SendData(url, data));
    }


    private IEnumerator SendData(string url, Data data)
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
            Debug.Log("Data sent to server successfully");
            Debug.Log($"PHP response: {response}");

            var serverResponse = JsonUtility.FromJson<Data>(response);
            
            // Wait for unity to dispose everything
            yield return new WaitForEndOfFrame();

            data.OnCreate(serverResponse.id);
        }
        Debug.Log("Ending data posting");
    }
}

