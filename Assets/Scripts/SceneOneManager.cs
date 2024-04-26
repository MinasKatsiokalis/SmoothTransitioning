using UnityEngine;
using MK.Transitioning.Utils;
using MK.Transitioning.Interfaces;

namespace MK.Transitioning
{   
    /// <summary>
    /// Scene manager for Scene1.
    /// Responsible for communicate with external resources and transfer event triggers.
    /// </summary>
    public class SceneOneManager : AbstractSceneManager
    {
        [SerializeField]
        private TextFader _textFader;

        private void OnEnable()
        {
            OnSceneLoaded();
        }

        private void Start()
        {
            _textFader.FadeIn();
        }

        private void OnDisable()
        {
            SceneTransition();
            OnSceneUnloaded();
        }

        private async void SceneTransition()
        {   
            var parent = Utilities.GetTopParent(_textFader.Transform);
            DontDestroyOnLoad(parent);

            await _textFader.FadeOutAwaitable();
            Destroy(parent.gameObject);
        }
    }
}
