using System.Threading.Tasks;
using UnityEngine;

namespace MK.Transitioning.Utils
{
    public static class Utilities
    {
        /// <summary>
        /// Get the top parent of a transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform GetTopParent(Transform transform)
        {
            Transform topParent = transform;
            while (topParent.parent != null)
                topParent = topParent.parent;

            return topParent;
        }

        /// <summary>
        /// Moves the <paramref name="transform"/> to the center of the screen at <paramref name="distance"/> units.
        /// Interpolates the movement using <paramref name="speed"/>.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="speed"></param>
        /// <param name="distance"></param>
        public static async void MoveToCenter(Transform transform,float speed, float distance)
        {
            var camera = Camera.main.transform;
            Vector3 targetPosition = camera.position + camera.forward * distance;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            while (distanceToTarget > 0.001)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
                distanceToTarget = Vector3.Distance(transform.position, targetPosition);

                await Task.Yield();
            }
        }

        /// <summary>
        /// Changes the surface type of a material.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="surfaceType"></param>
        public static void ChnageSurfaceType(Material material, SurfaceType surfaceType)
        {
            switch (surfaceType)
            {
                case SurfaceType.Opaque:
                    material.SetFloat("_Surface", 0);
                    break;
                case SurfaceType.Transparent:
                    material.SetFloat("_Surface", 1);
                    break;
            }
        }
    }

    public enum SurfaceType
    {
        Opaque = 0,
        Transparent = 1
    }
}
