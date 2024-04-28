using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MK.Transitioning.Core
{
    public class InputManager : MonoBehaviour
    {
        #region Properties
        public static InputManager Instance { get; private set; }

        public static event Action<Vector2> OnLeftClick;
        public static event Action<Vector2> OnLeftClickDrag;
        public static event Action<Vector2> OnRightClickDrag;
        public static event Action<Vector2> OnScroll;
        public static event Action<Vector2> OnMousePositionChanged;
        
        private InputActions.MouseInputActions mouseInputActions;

        private const float MaxTapMoveDistance = 15.0f; //Pixels
        private const float MaxTapTime = 0.5f; //Seconds

        private Vector2 mousePosition;
        private Vector2 previousMousePosition;
        private Vector2 tapStartPosition;
        private float tapStartTime;

        private bool isRightClickPressed = false;
        private bool isLeftClickPressed = false;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            mouseInputActions = new InputActions().MouseInput;
        }

        private void OnEnable()
        {
            mouseInputActions.Enable();

            #region Mouse Position
            //Get the mouse position
            mouseInputActions.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
            #endregion

            #region Scroll
            //Get the scroll click event
            mouseInputActions.Scroll.performed += ctx => OnScroll?.Invoke(ctx.ReadValue<Vector2>());
            #endregion

            #region Right Click
            //Get the right click event
            mouseInputActions.RightClick.started += _ => isRightClickPressed = true;
            mouseInputActions.RightClick.canceled += _ => isRightClickPressed = false;
            #endregion

            #region Left Click
            mouseInputActions.LeftClick.started += _ =>
            {
                isLeftClickPressed = true;
                tapStartTime = Time.time;
                tapStartPosition = mousePosition;
            };
            mouseInputActions.LeftClick.canceled += _ =>
            {
                isLeftClickPressed = false;
                if(IsTap(mousePosition))
                    OnLeftClick?.Invoke(mousePosition);
            };
            #endregion
        }

        private void OnDisable() => mouseInputActions.Disable();

        private void OnDestroy() => mouseInputActions.Disable();

        private void FixedUpdate()
        {   
            //Check right click drag
            if(isRightClickPressed)
                OnRightClickDrag?.Invoke(mousePosition - previousMousePosition);
            //Check left click drag
            if(isLeftClickPressed)
                OnLeftClickDrag?.Invoke(mousePosition - previousMousePosition);
            //Check if the mouse position has changed
            if(Vector2.Distance(mousePosition,previousMousePosition) != 0)
                OnMousePositionChanged?.Invoke(mousePosition);
            //Update the previous mouse position
            previousMousePosition = mousePosition;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Compares the <paramref name="screenPosition"/> of the touch with the initial position of the touch.
        /// Compares the time of the touch with the initial time of the touch.
        /// </summary>
        /// <param name="touchId"></param>
        /// <returns>True if the touch is a tap.</returns>
        private bool IsTap(Vector2 screenPosition)
        {
            float distance = Vector2.Distance(tapStartPosition, screenPosition);
            float duration = Time.time - tapStartTime;

            return (distance <= MaxTapMoveDistance && duration <= MaxTapTime);
        }
        #endregion
    }
}