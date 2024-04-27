using UnityEngine;
using MK.Transitioning.Utils;
using MK.Transitioning.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MK.Transitioning
{   
    /// <summary>
    /// Scene manager for Scene1.
    /// Responsible for communicate with external resources and transfer event triggers.
    /// </summary>
    public class SceneOneManager : AbstractSceneManager
    {
        #region Unity Methods
        private void Start()
        {
            FadeInObjects();
            CountDownToTransition(5);
        }

        private new void OnDisable()
        {   
            SceneTransition();
            base.OnDisable();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executes when the scene is about to transition.
        /// </summary>
        private async void SceneTransition()
        {   
            DontDestroyOnLoad(ObjectsContainer);
            await FadeOutObjects();
            Destroy(ObjectsContainer);
        }

        /// <summary>
        /// Counts down to trigger the transition to the next scene.
        /// </summary>
        /// <param name="seconds"></param>
        private async void CountDownToTransition(int seconds)
        {
            await Task.Delay(seconds * 1000);
            EventSystem.SceneEvents.OnTransitionTriggered?.Invoke(this.Scene);
        }
        #endregion
    }
}
