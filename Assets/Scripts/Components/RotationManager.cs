using UnityEngine;
using MK.Transitioning.Interfaces;

namespace MK.Transitioning.Core
{
    public class RotationManager : MonoBehaviour
    {

        #region Properties
        //Only one instance of SelectionManager is allowed.
        public static RotationManager Instance { get; private set; }

        //Current object that is being rotated.
        private Transform currentObject = null;
        //Raycast components
        private Camera mainCamera;
        private RaycastHit[] raycastHits = new RaycastHit[1];
        //Mouse values
        private Vector2 mousePosition;
        private Vector2 mouseDelta;
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
            InputManager.OnLeftClickDragStarted += OnLeftDrag;
            InputManager.OnMousePositionChanged += MousePositionChanged;
            InputManager.OnLeftClickDragStopped += NullifyCurrentObject;
        }

        private void OnDestroy()
        {
            InputManager.OnLeftClickDragStarted -= OnLeftDrag;
            InputManager.OnMousePositionChanged -= MousePositionChanged;
            InputManager.OnLeftClickDragStopped -= NullifyCurrentObject;
        }

        private void Start()
        {
            enabled = (InputManager.Instance != null);
            if (!enabled)
                Debug.LogError("InputManager is missing.");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// If the user is dragging the left mouse button starts rotating the object.
        /// </summary>
        /// <param name="mouseDelta"></param>
        private void OnLeftDrag(Vector2 mouseDelta)
        {   
            this.mouseDelta = mouseDelta;
            if(currentObject != null)
                return;

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits);
            if (hitCount > 0)
            {   
                var hittedObject = raycastHits[0].transform;
                currentObject = hittedObject.TryGetComponent(out IRotatable rotatable)? rotatable.Transform : null;
            }
        }

        /// <summary>
        /// Sets the mouse position.
        /// If there is a current object, rotates it.
        /// </summary>
        /// <param name="mousePosition"></param>
        private void MousePositionChanged(Vector2 mousePosition) 
        { 
            this.mousePosition = mousePosition;
            if(currentObject != null)
                currentObject.Rotate(mouseDelta);
        }

        /// <summary>
        /// Sets Current object to null.
        /// </summary>
        private void NullifyCurrentObject() => currentObject = null;
        #endregion
    }
}
