using System;
using UnityEngine;

namespace TPSHorror.Interaction
{
    public interface IInteractAble
    {
        Vector3 UiOffset { get;}
        
        Vector3 Pos{get;}

        void StartInteract();
        void FinishedInteract();
        event EventHandler<IInteractAble> OnStartInteract;
        event EventHandler<IInteractAble> OnFinishedInteract;
    }
}
