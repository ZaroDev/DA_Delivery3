using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System;
using UnityEngine.Rendering;
using UnityEngine.Networking;

namespace GameAnalytics.Tooling
{
    public static class DataGetter
    {
        private enum GetterType : int
        {
            GetPaths,
            GetDeaths,
        }
        private static readonly Dictionary<GetterType, string> Urls = new()
        {
            { GetterType.GetPaths, "https://citmalumnes.upc.es/~victorfz/GetAllPaths.php" },
            { GetterType.GetDeaths, "https://citmalumnes.upc.es/~victorfz/GetAllDeaths.php"}
        };

        public static void GetPaths(Action<string> callback)
        {
            GetData(Urls[GetterType.GetPaths], callback);
        }
        
        public static void GetDeaths(Action<string> callback)
        {
            GetData(Urls[GetterType.GetDeaths], callback);
        }
        
        private static void GetData(string url, Action<string>callback)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(GetDataRoutine(url, callback));
        }

        private static IEnumerator GetDataRoutine(string url, Action<string>callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + www.error);
                }
                else
                {
                    // Parse the JSON data received from the server
                    string jsonResult = www.downloadHandler.text;
                    callback?.Invoke(jsonResult);
                }
            }
        }
    }
}
