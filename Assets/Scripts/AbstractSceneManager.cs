using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using MK.Transitioning.Interfaces;

namespace MK.Transitioning
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
        }

        public void OnDisable()
        {
            Debug.Log($"{Scene.name} Unloaded");
            EventSystem.SceneEvents.OnSceneUnloaded?.Invoke(Scene);
        }

        /// <summary>
        /// Fade in all objects in the container.
        /// </summary>
        public void FadeInObjects()
        {
            var fadables = ObjectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;

            foreach (var fadable in fadables)
                fadable.FadeIn();
        }

        /// <summary>
        /// Fade out all objects in the container except the selected object.
        /// </summary>
        public async Task FadeOutObjects()
        {
            var fadables = ObjectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;

            List<Task> tasks = new List<Task>(fadables.Length - 1);
            foreach (var fadable in fadables)
                tasks.Add(fadable.FadeOutAwaitable());
            await Task.WhenAll(tasks);
        }
    }
}
