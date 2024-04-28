using Unity.VisualScripting;
using UnityEngine;

namespace MK.Transitioning.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraControler : MonoBehaviour
    {
        #region Properties
        [Header("Camera Rotation")]
        [Tooltip("Target for the camera to rotate around.")]
        public Transform target;
        [Tooltip("Speed of the camera rotation.")]
        public float rotateSpeed = 5.0f;
        [Tooltip("Smooth factor for camera rotation.")]
        public float smoothTime = 0.01f;

        [Header("Camera Zooming")]
        [Tooltip("Speed of zoom in/out")]
        public float zoomSpeed = 5f;
        [Tooltip("Minimum zoom level")]
        public float minZoom = 10f;
        [Tooltip("Maximum zoom level")]
        public float maxZoom = 90f;

        private Camera mainCamera;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            InputManager.OnRightClickDrag += OnClick;
            InputManager.OnScroll += Zooming;
        }

        private void OnDisable()
        {
            InputManager.OnRightClickDrag -= OnClick;
            InputManager.OnScroll -= Zooming;
        }

        void Start()
        {
            mainCamera = GetComponent<Camera>();

            if (target == null)
                Debug.LogError("CameraControler: No target set for the camera to rotate around.");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if the user is zooming in/out and updates the camera field of view.
        /// </summary>
        private void Zooming(Vector2 delta)
        {
            float scrollData = delta.y;
            float desiredZoom = mainCamera.fieldOfView - scrollData * zoomSpeed;
            desiredZoom = Mathf.Clamp(desiredZoom, minZoom, maxZoom);

            // Interpolate the field of view
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, desiredZoom, Time.deltaTime * zoomSpeed);
        }

        /// <summary>
        /// Rotates the camera around the target based on the mouse movement.
        /// </summary>
        private void OnClick(Vector2 mouseDelta)
        {
            float horizontal = mouseDelta.x * rotateSpeed * Time.deltaTime;
            float vertical = mouseDelta.y * rotateSpeed * Time.deltaTime;

            // Calculate the new position
            transform.RotateAround(target.position, Vector3.up, horizontal);
            transform.RotateAround(target.position, transform.right, -vertical);

            // Ensure the camera is always looking at the target
            transform.LookAt(target);
        }
        #endregion
    }
}
