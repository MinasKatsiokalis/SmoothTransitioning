using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{   
    /// <summary>
    /// Is resposible for selecting objects in any scene.
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {   
        //Only one instance of SelectionManager is allowed.
        public static SelectionManager Instance { get; private set; }

        //Is the SelectionManager enabled?
        private bool isEnabled;
        public bool IsEnabled
        {
            set => isEnabled = value;
            get => isEnabled;
        }

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable() => EventSystem.SceneEvents.OnSceneLoaded += EnableBehaviour;
        
        private void OnDisable() => EventSystem.SceneEvents.OnSceneLoaded -= EnableBehaviour;
        
        private void Start() => isEnabled = false;

        void Update()
        {   
            if(!isEnabled)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log($"Selected: {hit.transform.gameObject.name}");
                    EventSystem.SelectionEvents.OnObjectSelected?.Invoke(hit.transform.gameObject);
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Enables the SelectionManager if the scene is the second scene.
        /// </summary>
        /// <param name="scene"></param>
        private void EnableBehaviour(Scene scene) => isEnabled = (scene.buildIndex == 2 || scene.buildIndex == 3) ? true : false;
        #endregion
    }
}
