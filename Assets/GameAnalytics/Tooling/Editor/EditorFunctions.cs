#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAnalytics.Tooling
{
    public static class EditorFunctions
    {
        [MenuItem("Game Analytics/Fill player Ids")]
        public static void FillPlayerIDs()
        {
            var entityTrackers = GameObject.FindObjectsByType<EntityTracker>(FindObjectsSortMode.None);

            foreach (var entityTracker in entityTrackers)
            {
                entityTracker.RequestID();
            }
        }
    }
}
#endif