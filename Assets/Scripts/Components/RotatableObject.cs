using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning.Components
{
    public class RotatableObject : MonoBehaviour, IRotatable
    {
        #region Properties
        public Transform Transform => transform;

        [Range(0.0f, 10.0f)]
        [SerializeField]
        private float _rotationSpeed = 10f;
        public float RotationSpeed 
        { 
            get => _rotationSpeed; 
            set => _rotationSpeed = value;
        }
        #endregion

        #region Public Methods
        public void Rotate(Vector2 mouseDelta) => transform.Rotate(mouseDelta.y * RotationSpeed, -mouseDelta.x * RotationSpeed, 0, Space.World);
        #endregion
    }
}
