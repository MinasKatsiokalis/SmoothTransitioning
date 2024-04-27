using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning
{
    /// <summary>
    /// Scene manager for Scene3.
    /// Responsible for communicate with external resources and transfer event triggers.
    /// </summary>
    public class SceneThreeManager : AbstractSceneManager
    {
        #region Unity Methods
        private new void OnEnable()
        {   
            EventSystem.SceneEvents.OnObjectsTransfered += TransferObjects;
            base.OnEnable();
        }

        private new void OnDisable()
        {
            EventSystem.SceneEvents.OnObjectsTransfered -= TransferObjects;
            base.OnDisable();
        }

        void Start()
        {
            FadeInObjects();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Transfers the objects to the <see cref="SceneThreeManager.ObjectsContainer"/>.
        /// </summary>
        /// <param name="objects"></param>
        private void TransferObjects(GameObject[] objects)
        {
            Debug.Log("Objects Transfered");
            foreach (var obj in objects)
                obj.transform.SetParent(ObjectsContainer.transform);
        }
        #endregion
    }
}
