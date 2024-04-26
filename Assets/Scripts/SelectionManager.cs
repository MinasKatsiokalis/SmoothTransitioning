using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    public class SelectionManager : MonoBehaviour
    {   
        public static SelectionManager Instance { get; private set; }

        public static event Action<GameObject> OnObjectSelected;

        private bool isEnabled;

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
                    Debug.Log("Selected object: " + hit.transform.gameObject.name);
                    OnObjectSelected?.Invoke(hit.transform.gameObject);
                }
            }
        }

        private void EnableBehaviour(Scene scene) => isEnabled = (scene.buildIndex == 2) ? true : false;
    }
}
