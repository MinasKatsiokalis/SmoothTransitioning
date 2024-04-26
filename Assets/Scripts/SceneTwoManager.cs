using MK.Transitioning.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    public class SceneTwoManager : AbstractSceneManager
    {
        [SerializeField]
        private GameObject objectsContainer;
        [SerializeField]
        private GameObject selectedObject;

        private void OnEnable()
        {
            SelectionManager.OnObjectSelected += SceneTransition;
            EventSystem.SceneEvents.OnSceneLoaded += MoveObject;

            OnSceneLoaded();
        }

        private void Start()
        {
            FadeInObjects();
        }

        private void OnDisable()
        {
            foreach (Transform child in objectsContainer.transform)
                if(child != selectedObject.transform)
                    Destroy(child.gameObject);

            SelectionManager.OnObjectSelected -= SceneTransition;
            EventSystem.SceneEvents.OnSceneLoaded -= MoveObject;

            OnSceneUnloaded();
        }

        private void FadeInObjects()
        {
            var fadables = objectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;

            foreach (var fadable in fadables)
                fadable.FadeIn();
        }

        private void FadeOutObjects()
        {
            var fadables = objectsContainer.GetComponentsInChildren<IFadable>();
            if (fadables.Length <= 0)
                return;

            foreach (var fadable in fadables)
            {
                if(fadable.Transform == selectedObject.transform)
                    continue;

                fadable.FadeOut();
            }
        }

        private void SceneTransition(GameObject selectedObject)
        {
            this.selectedObject = selectedObject;

            FadeOutObjects();
            this.selectedObject.transform.SetParent(null);
            selectedObject.GetComponent<IOrbitable>().StopOrbit();
            //DontDestroyOnLoad(this.selectedObject);
            EventSystem.SceneEvents.OnTransitionTriggered?.Invoke(gameObject.scene);
        }

        private void MoveObject(Scene scene)
        {
            if (scene.buildIndex != 3)
                return;

            SceneManager.MoveGameObjectToScene(selectedObject, scene);
            selectedObject.transform.position = Vector3.Lerp(transform.position, new Vector3(0,0,3.5f), Time.deltaTime * 0.01f);
        }
    }
}
