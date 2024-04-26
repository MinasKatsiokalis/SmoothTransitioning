using TMPro;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using MK.Transitioning.Interfaces;

namespace MK.Transitioning
{
    /// <summary>
    /// Behaviour class that fades in and out a text component.
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(TMP_Text))]
    public class TextFader : MonoBehaviour, IFadable
    {
        #region Properties
        //Reference to the transform
        public Transform Transform => transform;

        //Text and background image to fade
        [SerializeField]
        private TMP_Text _textComponent = null;
        public TMP_Text TextComponent
        {
            get => _textComponent;
            set => _textComponent = value;
        }

        [SerializeField]
        private Image _backgroundImage = null;
        public Image BackgroundImage
        {
            get => _backgroundImage;
            set => _backgroundImage = value;
        }

        //Alpha values for text and background when fade in
        [SerializeField]
        [Tooltip("Desired aplha value of text component")]
        [Range(0.0f, 1.0f)]
        private float _textAlpha = 1f;
        public float TextAlpha
        {
            get => _textAlpha;
            set => _textAlpha = value;
        }

        [SerializeField]
        [Tooltip("Desired aplha value of background component")]
        [Range(0.0f, 1.0f)]
        private float _backgroundAlpha = 0.6f;
        public float BackgroundAlpha
        {
            get => _backgroundAlpha;
            set => _backgroundAlpha = value;
        }

        //Duration of fade effect
        [SerializeField]
        [Tooltip("Duration of fade in/out effect in seconds")]
        private float _duration = 1.0f;
        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fades in the text.
        /// </summary>
        public async void FadeIn() => await FadeTask(_textAlpha, _backgroundAlpha);
        public async Task FadeInAwaitable() => await FadeTask(_textAlpha, _backgroundAlpha);

        /// <summary>
        /// Fades out the text. 
        /// </summary>
        public async void FadeOut() => await FadeTask(0f, 0f);
        public async Task FadeOutAwaitable() => await FadeTask(0f, 0f);
        #endregion

        #region Private Methods
        /// <summary>
        /// Coroutine that fades in or out the text smoothly.
        /// </summary>
        /// <param name="fadeIn"></param>
        /// <param name="targetTextAlpha"></param>
        /// <param name="targetBackgroundAlpha"></param>
        /// <returns></returns>
        private async Task FadeTask(float targetTextAlpha, float targetBackgroundAlpha)
        {
            float textAlphaDiff = Math.Abs(_textComponent.color.a - targetTextAlpha);
            float backgroundAlphaDiff = Math.Abs(_backgroundImage.color.a - targetBackgroundAlpha);

            while (textAlphaDiff > 0f || backgroundAlphaDiff > 0f)
            {
                if (textAlphaDiff > 0f)
                {
                    float newAlpha = Mathf.MoveTowards(_textComponent.color.a, targetTextAlpha, Time.deltaTime / Duration);
                    _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, newAlpha);
                    textAlphaDiff = Math.Abs(_textComponent.color.a - targetTextAlpha);
                }

                if (backgroundAlphaDiff > 0f)
                {
                    float newAlpha = Mathf.MoveTowards(_backgroundImage.color.a, targetBackgroundAlpha, Time.deltaTime / Duration);
                    _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, newAlpha);
                    backgroundAlphaDiff = Math.Abs(_backgroundImage.color.a - targetBackgroundAlpha);
                }
                await Task.Yield();
            }
        }
        #endregion
    }
}
