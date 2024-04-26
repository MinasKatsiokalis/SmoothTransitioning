using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    public static class EventSystem
    {
        public static class SceneEvents
        {
            public static Action<Scene> OnSceneLoaded;
            public static Action<Scene> OnSceneUnloaded;
            public static Action<Scene> OnTransitionTriggered;
        }

        public static class TimelineEvents
        {
            public static Action OnSceneOneSelected;
            public static Action OnSceneTwoSelected;
            public static Action OnSceneThreeSelected;
        }
    }
}
