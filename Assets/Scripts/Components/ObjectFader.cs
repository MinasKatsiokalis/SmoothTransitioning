using UnityEngine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MK.Transitioning.Interfaces;
using System;
using MK.Transitioning.Utils;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace MK.Transitioning.Components
{
    public class ObjectFader : MonoBehaviour, IFadable
    {
        #region Properties
        //Reference to the transform
        public Transform Transform => transform;

        [SerializeField]
        [Tooltip("Desired aplha value")]
        [Range(0.0f, 1.0f)]
        private float _targetAlpha = 1.0f;
        public float TargetAlpha
        {
            get => _targetAlpha;
            set => _targetAlpha = value;
        }

        //Duration of fade effect
        [SerializeField]
        [Tooltip("Duration of fade in/out effect in seconds")]
        private float _duration = 2.0f;
        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        //Materials to fade
        private Material[] materials = null;
        //Task management
        private Task fadeTask = null;
        private CancellationTokenSource cancellationTokenSource = null;
        #endregion

        #region Unity Methods
        private void Start()
        {
            materials = GetComponentsInChildren<Renderer>().Select(renderer => renderer.material).ToArray();
        }

        private void OnDisable()
        {
            if (fadeTask != null && !fadeTask.IsCompleted)
            {       
                Debug.Log("Disable");
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fades in the object (Async).
        /// </summary>
        public async void FadeIn() => await Fade(_targetAlpha);

        /// <summary>
        /// Fades in the object (Async/Awaitable).
        /// </summary>
        /// <returns>Task</returns>
        public async Task FadeInAwaitable() => await Fade(_targetAlpha);

        /// <summary>
        /// Fades out the object (Async). 
        /// </summary>
        public async void FadeOut() => await Fade(0f);

        /// <summary>
        /// Fades out the object (Async/Awaitable).
        /// </summary>
        /// <returns>Task</returns>
        public async Task FadeOutAwaitable() => await Fade(0f);
        #endregion

        #region Private Methods
        /// <summary>
        /// Manages FadeTask operations and handles cancellation.
        /// </summary>
        /// <param name="targetAlpha"></param>
        /// <returns></returns>
        private async Task Fade(float targetAlpha)
        {
            // If a fade task is running, cancel it
            if (fadeTask != null && !fadeTask.IsCompleted)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
            
            // Create a new CancellationTokenSource
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await (fadeTask = FadeTask(targetAlpha, cancellationTokenSource.Token));
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Fade cancelled");
                if(materials == null || cancellationTokenSource.IsCancellationRequested)
                    return;

                SetAlpha(targetAlpha);
                cancellationTokenSource.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log($"An Exception occured: {e.Message}");
            }
        }

        /// <summary>
        /// Coroutine that fades in or out the text smoothly.
        /// </summary>
        /// <param name="fadeIn"></param>
        /// <param name="targetTextAlpha"></param>
        /// <param name="targetBackgroundAlpha"></param>
        /// <returns></returns>
        private async Task FadeTask(float targetAlpha, CancellationToken token)
        {
            if (materials == null || materials.Length == 0)
                return;

            Color color = materials[0].color;
            float alphaDiff = Mathf.Abs(color.a - targetAlpha);

            while (alphaDiff > 0f)
            {
                if (token.IsCancellationRequested)
                    throw new TaskCanceledException();

                float newAlpha = Mathf.MoveTowards(materials[0].color.a, targetAlpha, Time.deltaTime / Duration);
                foreach (var material in materials)
                    material.color = new Color(color.r, color.g, color.b, newAlpha);
                alphaDiff = Mathf.Abs(materials[0].color.a - targetAlpha);
                
                await Task.Yield();
            }
        }

        /// <summary>
        /// Sets an <paramref name="alpha"/> value to all object materials.
        /// </summary>
        /// <param name="alpha"></param>
        private void SetAlpha(float alpha)
        {
            foreach (var material in materials)
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
        }
        #endregion
    }
}
