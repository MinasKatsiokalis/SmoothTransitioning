using MK.Transitioning.Interfaces;
using UnityEngine;

namespace MK.Transitioning.Core
{   
    /// <summary>
    /// Is resposible for selecting objects in any scene.
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        #region Properties
        //Only one instance of SelectionManager is allowed.
        public static SelectionManager Instance { get; private set; }

        //Is the SelectionManager enabled?
        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
        }

        private Camera mainCamera;
        private RaycastHit[] raycastHits = new RaycastHit[1];
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            InputManager.OnLeftClick += OnClick;
        }

        private void Start() => EnableBehaviour(true);
        #endregion

        #region Public Methods
        public void EnableBehaviour(bool enable) => isEnabled = enable;
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if the user clicked on an object and invokes the OnObjectSelected event.
        /// </summary>
        private void OnClick(Vector2 mousePosition)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;
                
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits);

            if (hitCount > 0)
            {   
                var gameObject = raycastHits[0].transform.gameObject;
                if(gameObject.TryGetComponent(out ISelectable selectable))
                {
                    selectable.OnSelected();
                    EventSystem.SelectionEvents.OnObjectSelected?.Invoke(gameObject);
                }
            }
        }
        #endregion
    }
}
