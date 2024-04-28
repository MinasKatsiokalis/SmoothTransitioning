using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning.Core
{
    public static class EventSystem
    {
        public static class SceneEvents
        {
            public static Action<Scene> OnSceneLoaded;
            public static Action<Scene> OnSceneUnloaded;
            public static Action<Scene> OnTransitionTriggered;
            public static Action<GameObject[]> OnObjectsTransfered;
        }

        public static class TimelineEvents
        {
            public static Action OnSceneOneSelected;
            public static Action OnSceneTwoSelected;
            public static Action OnSceneThreeSelected;
        }

        public static class SelectionEvents
        {
            public static Action<GameObject> OnObjectSelected;
        }
    }
}
