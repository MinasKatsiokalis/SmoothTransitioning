using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MK.Transitioning.Core
{
    public class InputManager : MonoBehaviour
    {
        #region Properties
        //Only one instance of InputManager is allowed.
        public static InputManager Instance { get; private set; }

        #region Input Events
        //Fat left-mouse click
        public static event Action<Vector2> OnLeftClick;
        //Left-mouse drag
        public static event Action<Vector2> OnLeftClickDragStarted;
        public static event Action OnLeftClickDragStopped;
        //Right-mouse drag
        public static event Action<Vector2> OnRightClickDragStarted;
        public static event Action OnRightClickDragStopped;
        //Wheel scroll
        public static event Action<Vector2> OnScroll;
        //Mouse position
        public static event Action<Vector2> OnMousePositionChanged;
        #endregion

        #region Public
        private static InputActions.MouseInputActions mouseInputActions;
        public static InputActions.MouseInputActions MouseInputActions => mouseInputActions;

        private static Vector2 mousePosition;
        public static Vector2 MousePosition => mousePosition;
        #endregion

        #region Private
        private const float MaxTapMoveDistance = 15.0f; //Pixels
        private const float MaxTapTime = 0.5f; //Seconds

        private Vector2 previousMousePosition;
        private Vector2 tapStartPosition;
        private float tapStartTime;

        private bool isRightClickPressed = false;
        private bool isLeftClickPressed = false;

        private Func<Vector2,Vector2,float> getDistance = (a,b) => Vector2.Distance(a,b);
        #endregion

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
            mouseInputActions.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
            #endregion

            #region Scroll
            mouseInputActions.Scroll.performed += ctx => OnScroll?.Invoke(ctx.ReadValue<Vector2>());
            #endregion

            #region Right Click
            mouseInputActions.RightClick.started += _ =>
            {
                isRightClickPressed = true;
            };
            mouseInputActions.RightClick.canceled += _ =>
            {
                isRightClickPressed = false;
                OnRightClickDragStopped?.Invoke();
            };
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
                else
                    OnLeftClickDragStopped?.Invoke();
            };
            #endregion
        }

        private void OnDisable() => mouseInputActions.Disable();

        private void OnDestroy() => mouseInputActions.Disable();

        private void FixedUpdate()
        {
            //Send the mouse position
            if(previousMousePosition != mousePosition)
                OnMousePositionChanged?.Invoke(mousePosition);
            //Check right click drag
            if (isRightClickPressed)
                OnRightClickDragStarted?.Invoke(mousePosition - previousMousePosition);
            //Check left click drag
            if (isLeftClickPressed)
            {
                if(getDistance(mousePosition, tapStartPosition) <= MaxTapMoveDistance)
                    return;
                OnLeftClickDragStarted?.Invoke(mousePosition - previousMousePosition);
            }
            //Update the previous mouse position
            previousMousePosition = mousePosition;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Compares the <paramref name="screenPosition"/> of the mouse posiiton with the initial position of the click.
        /// Compares the time of the click with the initial time of the click.
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