using MK.Transitioning.Interfaces;
using System.Collections;
using UnityEngine;

namespace MK.Transitioning.Components
{
    public class Orbiter : MonoBehaviour, IOrbitable
    {
        #region Properties
        [SerializeField]
        [Tooltip("The target to orbit around")]
        private Transform _orbitPoint;
        public Vector3 OrbitPointPosition
        {
            get => _orbitPoint.position;
            set => _orbitPoint.position = value;
        }

        [SerializeField]
        [Tooltip("Speed of orbit")]
        private float _speed = 20f;
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [SerializeField]
        [Tooltip("Distance from orbit target")]
        private float _distance = 1f;
        public float Distance
        {
            get => _distance;
            set => _distance = value;
        }

        //Keep track of orbiting state
        private Coroutine orbitCoroutine;
        private Transform orbiter;
        #endregion

        #region Unity Methods
        private void Start()
        {
            orbiter = new GameObject("OrbiterPosition").transform;
            StartOrbit();
        }

        private void OnDisable()
        {
            StopOrbit();
        }

        private void OnDestroy()
        {
            StopOrbit();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start orbiting around target
        /// </summary>
        public void StartOrbit()
        {       
            if(orbitCoroutine == null)
                orbitCoroutine = StartCoroutine(Orbiting());
        }

        /// <summary>
        /// Stop orbiting around target
        /// </summary>
        public void StopOrbit()
        {
            if (orbitCoroutine == null)
                return;

            StopCoroutine(orbitCoroutine);
            try
            {
                Destroy(orbiter.gameObject);
            }
            catch
            {
                Debug.LogWarning("Orbiter object already destroyed");
            }
            finally
            {
                orbitCoroutine = null;
                Debug.Log($"Stopping Orbit {gameObject.name}");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Coroutine that orbits transform
        /// </summary>
        /// <returns></returns>
        private IEnumerator Orbiting()
        {
            orbiter.position = transform.position;
            orbiter.SetParent(_orbitPoint);

            while (orbiter != null)
            {
                orbiter.transform.RotateAround(_orbitPoint.position, _orbitPoint.up, _speed * Time.deltaTime);
                this.transform.position = orbiter.position;
                yield return null;
            }
            Debug.Log($"Stopping Orbit {gameObject.name}");
        }
        #endregion
    }
}
