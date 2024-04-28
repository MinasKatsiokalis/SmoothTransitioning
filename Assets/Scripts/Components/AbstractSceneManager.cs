using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using MK.Transitioning.Interfaces;
using MK.Transitioning.Core;

namespace MK.Transitioning.Components
{
    public abstract class AbstractSceneManager : MonoBehaviour
    {
        public GameObject ObjectsContainer;
        public Scene Scene { private set; get; }

        public void Awake()
        {
            Scene = this.gameObject.scene;
        }

        public void OnEnable()
        {
            Debug.Log($"{Scene.name} Loaded");
            EventSystem.SceneEvents.OnSceneLoaded?.Invoke(Scene);
            DontDestroyOnLoad(ObjectsContainer);
        }

        public void OnDisable()
        {
            EventSystem.SceneEvents.OnSceneUnloaded?.Invoke(Scene);
            Debug.Log($"{Scene.name} Unloaded");
        }

        /// <summary>
        /// Fade in all objects in the container.
        /// </summary>
        public void FadeInObjects()
        {   

            var fadables = ObjectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;
            Debug.Log($"Fading In {Scene.name}");
            foreach (var fadable in fadables)
                fadable.FadeIn();
        }

        /// <summary>
        /// Fade out all objects in the container except the selected object.
        /// </summary>
        public async Task FadeOutObjects()
        {   
            Debug.Log($"Fading Check {Scene.name}");
            var fadables = ObjectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;

            Debug.Log($"Fading Out {Scene.name}");
            List<Task> tasks = new List<Task>(fadables.Length);
            foreach (var fadable in fadables)
                tasks.Add(fadable.FadeOutAwaitable());
            await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Executes when the scene is about to transition.
        /// </summary>
        public async void SceneTransition()
        {
            if (ObjectsContainer == null)
            {
                Debug.Log($"CONTAINER = NULL IN {Scene.name}");
                return;
            }
            await FadeOutObjects();
            Destroy(ObjectsContainer);
        }
    }
}
