using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Transitioning
{
    public interface ISelectable
    {   
        Transform Transform { get; }
        bool IsSelected { get; set; }
        void OnSelected();
    }
}
