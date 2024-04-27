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
            EventSystem.SceneEvents.OnTransitionTriggered += (_) => GoToNextState();
            EventSystem.TimelineEvents.OnSceneOneSelected += () => ChangeState(State.Scene1);
            EventSystem.TimelineEvents.OnSceneTwoSelected += () => ChangeState(State.Scene2);
            EventSystem.TimelineEvents.OnSceneThreeSelected += () => ChangeState(State.Scene3);
        }

        private void Start()
        {   
            ChangeState(State.Scene1);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Changes the CurrentState to the next state in the order (linearly).
        /// </summary>
        private void GoToNextState()
        {
            int nextStateIndex = ((int)CurrentState % Enum.GetValues(typeof(State)).Length)+1;
            ChangeState((State)nextStateIndex);
        }

        /// <summary>
        /// Changes the CurrentState with <paramref name="newState"/> and load the respective scene. 
        /// </summary>
        /// <param name="newState"></param>
        private void ChangeState(State newState)
        {   
            Debug.Log($"Changing State: {CurrentState} -> {newState}");
            CurrentState = newState;
            
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)CurrentState, LoadSceneMode.Additive);
            operation.allowSceneActivation = true;
            operation.completed += _ => UnloadPreviousScene();
        }

        /// <summary>
        /// Unloads the previous scene (if loaded) based on the current state.
        /// </summary>
        private void UnloadPreviousScene()
        {   
            switch (CurrentState)
            {
                case State.Scene1:
                    {
                        if (SceneManager.GetSceneByBuildIndex((int)State.Scene3).isLoaded)
                            SceneManager.UnloadSceneAsync((int)State.Scene3);
                        Debug.Log($"Unloading Previous Scene: {(int)State.Scene3}");
                    }
                    break;
                case State.Scene2:
                    {
                        if (SceneManager.GetSceneByBuildIndex((int)State.Scene1).isLoaded)
                            SceneManager.UnloadSceneAsync((int)State.Scene1);
                        Debug.Log($"Unloading Previous Scene: {(int)State.Scene1}");
                    }
                    break;
                case State.Scene3:
                    {
                        if (SceneManager.GetSceneByBuildIndex((int)State.Scene2).isLoaded)
                            SceneManager.UnloadSceneAsync((int)State.Scene2);
                        Debug.Log($"Unloading Previous Scene: {(int)State.Scene2}");
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    [Serializable]
    public enum State
    { 
        Scene1 = 1, 
        Scene2 = 2, 
        Scene3 = 3
    }
}
