using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning.Interfaces
{
    public interface IRotatable
    {
        Transform Transform { get; }
        float RotationSpeed { get; set; }
        void Rotate(Vector2 delta);
    }
}
