using UnityEngine;
using MK.Transitioning.Core;

namespace MK.Transitioning.Components
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
        }

        private new void OnDisable()
        {   
            SceneTransition();
            base.OnDisable();
        }
        #endregion
    }
}
