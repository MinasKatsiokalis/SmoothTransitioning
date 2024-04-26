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
    }
}
