using MK.Transitioning.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace MK.Transitioning.Core
{
    public class RotationManager : MonoBehaviour
    {

        #region Properties
        //Only one instance of SelectionManager is allowed.
        public static RotationManager Instance { get; private set; }

        //Is the SelectionManager enabled?
        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
        }

        private Camera mainCamera;
        private RaycastHit[] raycastHits = new RaycastHit[1];
        private Vector2 mousePosition;
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
            InputManager.OnLeftClickDrag += OnLeftDrag;
            InputManager.OnMousePositionChanged += (mousePosition) => this.mousePosition = mousePosition;
        }

        private void Start() => EnableBehaviour(true);
        #endregion

        #region Public Methods
        public void EnableBehaviour(bool enable) => isEnabled = enable;
        #endregion

        #region Private Methods
        private void OnLeftDrag(Vector2 mouseDelta)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits);
            if (hitCount > 0)
            {
                var gameObject = raycastHits[0].transform.gameObject;
                if(gameObject.TryGetComponent(out IRotatable rotatable))
                    rotatable.Rotate(mouseDelta);
            }
        }
        #endregion
    }
}
