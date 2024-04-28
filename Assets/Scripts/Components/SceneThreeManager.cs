using UnityEngine;
using UnityEngine.UI;
using MK.Transitioning.Core;

namespace MK.Transitioning.Components
{
    /// <summary>
    /// Scene manager for Scene3.
    /// Responsible for communicate with external resources and transfer event triggers.
    /// </summary>
    public class SceneThreeManager : AbstractSceneManager
    {
        [SerializeField]
        private Button restartButton;

        #region Unity Methods
        private new void OnEnable()
        {   
            EventSystem.SceneEvents.OnObjectsTransfered += ObtainTransferedObjects;
            base.OnEnable();
        }
        void Start()
        {
            restartButton.onClick.AddListener(() => EventSystem.TimelineEvents.OnSceneOneSelected());
            FadeInObjects();
        }

        private new void OnDisable()
        {
            SceneTransition();
            EventSystem.SceneEvents.OnObjectsTransfered -= ObtainTransferedObjects;
            base.OnDisable();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Transfers the objects to the <see cref="SceneThreeManager.ObjectsContainer"/>.
        /// </summary>
        /// <param name="objects"></param>
        private void ObtainTransferedObjects(GameObject[] objects)
        {
            Debug.Log("Objects Transfered");
            foreach (var obj in objects)
                obj.transform.SetParent(ObjectsContainer.transform);
        }
        #endregion
    }
}
