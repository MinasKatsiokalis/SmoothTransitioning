using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using MK.Transitioning.Interfaces;
using System.Linq;

namespace MK.Transitioning
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
        #endregion

        #region Unity Methods
        private void Start()
        {
            materials = GetComponentsInChildren<Renderer>().Select(renderer => renderer.material).ToArray();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fades in the text.
        /// </summary>
        public async void FadeIn() => await FadeTask(_targetAlpha);
        public async Task FadeInAwaitable() => await FadeTask(_targetAlpha);

        /// <summary>
        /// Fades out the text. 
        /// </summary>
        public async void FadeOut() => await FadeTask(0f);
        public async Task FadeOutAwaitable() => await FadeTask(0f);
        #endregion

        #region Private Methods
        /// <summary>
        /// Coroutine that fades in or out the text smoothly.
        /// </summary>
        /// <param name="fadeIn"></param>
        /// <param name="targetTextAlpha"></param>
        /// <param name="targetBackgroundAlpha"></param>
        /// <returns></returns>
        private async Task FadeTask(float targetAlpha)
        {
            if (materials == null || materials.Length == 0)
                return;

            Color color = materials[0].color;
            float alphaDiff = Mathf.Abs(color.a - targetAlpha);

            while (alphaDiff > 0f)
            {
                float newAlpha = Mathf.MoveTowards(materials[0].color.a, targetAlpha, Time.deltaTime / Duration);
                foreach (var material in materials)
                    material.color = new Color(color.r, color.g, color.b, newAlpha);
                alphaDiff = Mathf.Abs(materials[0].color.a - targetAlpha);
                
                await Task.Yield();
            }
        }
        #endregion
    }
}
