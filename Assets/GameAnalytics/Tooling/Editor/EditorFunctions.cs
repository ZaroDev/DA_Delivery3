#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace GameAnalytics.Tooling
{
    public static class EditorFunctions
    {
        [MenuItem("Game Analytics/Fill player Ids")]
        public static void FillPlayerIDs()
        {
            var scene = SceneManager.GetActiveScene();
            var rootGOs = scene.GetRootGameObjects();

            var entityTrackers = rootGOs.Select(rootGo => rootGo.GetComponent<EntityTracker>())
                                                        .Where(tracker => tracker != null)
                                                        .ToList();

            foreach (var entityTracker in entityTrackers)
            {
                entityTracker.RequestID();
            }
        }
    }
}
#endif