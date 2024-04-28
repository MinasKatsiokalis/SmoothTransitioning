using UnityEngine;
using MK.Transitioning.Interfaces;

namespace MK.Transitioning.Components
{
    public class SelectableObject : MonoBehaviour, ISelectable
    {
        #region Unity Methods
        public Transform Transform => transform;

        private bool isSelected = false;
        public bool IsSelected => isSelected;
        #endregion

        #region Public Methods
        public void OnSelected()
        {
            Debug.Log($"Selected: {transform.name}");
            isSelected = !isSelected;
        }
        #endregion
    }
}
