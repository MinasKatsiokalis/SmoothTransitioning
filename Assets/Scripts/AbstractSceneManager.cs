using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    public abstract class AbstractSceneManager : MonoBehaviour
    {
        public Scene scene;

        public void Awake()
        {
            scene = this.gameObject.scene;
        }

        public void OnSceneLoaded()
        {   
            Debug.Log($"{scene.name} Loaded");
            EventSystem.SceneEvents.OnSceneLoaded?.Invoke(scene);
        }

        public void OnSceneUnloaded()
        {
            Debug.Log($"{scene.name} Unloaded");
            EventSystem.SceneEvents.OnSceneUnloaded?.Invoke(scene);
        }
    }
}
