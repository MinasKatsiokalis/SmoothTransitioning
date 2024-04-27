using MK.Transitioning.Interfaces;
using MK.Transitioning.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    /// <summary>
    /// Scene manager for Scene2.
    /// Responsible for communicate with external resources and transfer event triggers.
    /// </summary>
    public class SceneTwoManager : AbstractSceneManager
    {
        //Selected object to be moved to the next scene
        private GameObject selectedObject;

        #region Unity Methods
        private new void OnEnable()
        {   
            EventSystem.SelectionEvents.OnObjectSelected += SetSelectedObject;
            EventSystem.SceneEvents.OnSceneLoaded += TransferObject;
            base.OnEnable();
        }

        private void Start()
        {
            FadeInObjects();
        }

        private new void OnDisable()
        {
            SceneTransition();
            EventSystem.SelectionEvents.OnObjectSelected -= SetSelectedObject;
            EventSystem.SceneEvents.OnSceneLoaded -= TransferObject;
            base.OnDisable();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the selected object.
        /// Triggers the scene transition process.
        /// </summary>
        /// <param name="selectedObject"></param>
        private void SetSelectedObject(GameObject selectedObject)
        {
            this.selectedObject = selectedObject;
            EventSystem.SceneEvents.OnTransitionTriggered?.Invoke(this.Scene);
        }

        /// <summary>
        /// Moves the selected object to the new scene.
        /// </summary>
        /// <param name="scene"></param>
        private void TransferObject(Scene scene)
        {
            if (scene.buildIndex != 3)
                return;

            selectedObject.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(selectedObject, scene);
            Utilities.MoveToCenter(selectedObject.transform, 1.5f, 3.5f);
            EventSystem.SceneEvents.OnObjectsTransfered?.Invoke(new GameObject[] { selectedObject });
        }

        /// <summary>
        /// Executes when the scene is about to transition.
        /// </summary>
        private async void SceneTransition()
        {   
            DontDestroyOnLoad(ObjectsContainer);
            await FadeOutObjects();
            Destroy(ObjectsContainer);
        }
        #endregion
    }
}
