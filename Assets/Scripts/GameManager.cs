using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MK.Transitioning
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public State CurrentState { get; private set; }

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventSystem.SceneEvents.OnTransitionTriggered += (scene) => 
            { 
                Debug.Log($"Transition Triggered: {scene.buildIndex}");
                if(scene.buildIndex == 2) 
                    GoToNextState(); 
            };
        }

        private void Start()
        {   
            ChangeState(State.Scene1);

            if (CurrentState == State.Scene1)
                Invoke("GoToNextState", 5f);
        }
        #endregion

        public void GoToNextState()
        {
            int nextStateIndex = ((int)CurrentState + 1) % Enum.GetValues(typeof(State)).Length;
            ChangeState((State)nextStateIndex);
        }

        private void ChangeState(State newState)
        {   
            Debug.Log($"Changing State: {CurrentState} -> {newState}");
            CurrentState = newState;
            SceneManager.LoadSceneAsync((int)CurrentState+1, LoadSceneMode.Additive);

            switch (CurrentState)
            {
                case State.Scene1:
                    {
                        if (SceneManager.GetSceneByBuildIndex(3).isLoaded)
                            SceneManager.UnloadSceneAsync(3);
                    }
                    break;
                case State.Scene2:
                    {
                        if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
                            SceneManager.UnloadSceneAsync(1);
                    }
                    break;
                case State.Scene3:
                    {
                        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
                            SceneManager.UnloadSceneAsync(2);
                    }
                    break;
                default:
                    break;
            }
            //int previousStateIndex = ((int)CurrentState - 1) % Enum.GetValues(typeof(State)).Length;
        }
    }

    public enum State
    { 
        Scene1 = 0, 
        Scene2 = 1, 
        Scene3 = 2
    }
}
