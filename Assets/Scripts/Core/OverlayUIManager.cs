using UnityEngine;
using UnityEngine.UI;

namespace MK.Transitioning.Core
{
    public class OverlayUIManager : MonoBehaviour
    {
        #region Properties
        //Only one instance of SelectionManager is allowed.
        public static OverlayUIManager Instance { get; private set; }

        [SerializeField]
        private Slider _progressBar;
        [SerializeField]
        private Button _scene1button;
        [SerializeField]
        private Button _scene2button;
        [SerializeField]
        private Button _scene3button;

        private Image button1outline;
        private Image button2outline;
        private Image button3outline;
        #endregion

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
            EventSystem.SceneEvents.OnSceneLoaded += (scene) => SceneLoaded(scene.buildIndex);
        }

        void Start()
        {   
            button1outline = _scene1button.transform.GetChild(1).GetComponent<Image>();
            button2outline = _scene2button.transform.GetChild(1).GetComponent<Image>();
            button3outline = _scene3button.transform.GetChild(1).GetComponent<Image>();

            _scene1button.onClick.AddListener(() => {
                SceneLoaded(1);
                EventSystem.TimelineEvents.OnSceneOneSelected?.Invoke();
            });
            _scene2button.onClick.AddListener(() => {
                SceneLoaded(2);
                EventSystem.TimelineEvents.OnSceneTwoSelected?.Invoke();
            });
            _scene3button.onClick.AddListener(() => {
                SceneLoaded(3);
                EventSystem.TimelineEvents.OnSceneThreeSelected?.Invoke();
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the button properties and progress bar value when scene has changed.
        /// </summary>
        /// <param name="index"></param>
        private void SceneLoaded(int index)
        {
            SetProgressBar(index);
            EnableButtonsInteraction(index);
            switch (index)
            {
                case 1:
                    SetButtonColor(button1outline);
                    break;
                case 2:
                    SetButtonColor(button2outline);
                    break;
                case 3:
                    SetButtonColor(button3outline);
                    break;
            }
        }

        /// <summary>
        /// Sets the progress bar value.
        /// </summary>
        /// <param name="value"></param>
        private void SetProgressBar(float value)
        {
            _progressBar.value = value;
        }

        /// <summary>
        /// CHanges the color of the button outline.
        /// </summary>
        /// <param name="outline"></param>
        private void SetButtonColor(Image outline)
        {
            // Reset all buttons to white
            button1outline.color = Color.black;
            button2outline.color = Color.black;
            button3outline.color = Color.black;

            // Set the clicked button to green
            outline.color = Color.green;
        }

        /// <summary>
        /// Makes the buttons interactable.
        /// </summary>
        private void EnableButtonsInteraction(int index)
        {
            _scene1button.interactable = false;
            _scene2button.interactable = false;
            _scene3button.interactable = false;

            switch (index)
            {
                case 1:
                    _scene2button.interactable = true;
                    break;
                case 2:
                    _scene3button.interactable = true;
                    break;
                case 3:
                    _scene1button.interactable = true;
                    break;
            }
        }
        #endregion
    }
}
