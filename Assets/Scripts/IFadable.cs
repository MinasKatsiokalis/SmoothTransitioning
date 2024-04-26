using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MK.Transitioning.Interfaces
{
    /// <summary>
    /// Interface implemented by objects that can be faded in and out
    /// </summary>
    public interface IFadable
    {   
        Transform Transform { get; }
        float Duration { get; set; }

        void FadeIn();
        Task FadeInAwaitable();

        void FadeOut();
        Task FadeOutAwaitable();
    }
}
