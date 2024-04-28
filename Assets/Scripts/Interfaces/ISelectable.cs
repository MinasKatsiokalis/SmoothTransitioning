using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning.Interfaces
{
    public interface ISelectable
    {   
        Transform Transform { get; }
        bool IsSelected { get; }
        void OnSelected();
    }
}
