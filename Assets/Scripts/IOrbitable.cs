using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning.Interfaces
{
    /// <summary>
    /// Interface implemented by objects that can be orbited around a position
    /// </summary>
    public interface IOrbitable
    {
        Vector3 OrbitPointPosition { get; set; }
        float Speed { get; set; }
        float Distance { get; set; }

        public void StartOrbit();
        public void StopOrbit();
    }
}
