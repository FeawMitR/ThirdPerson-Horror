using System;
using UnityEngine;
using TPSHorror.PlayerControllerCharacter;

namespace TPSHorror.Interaction
{
    public interface IInteractAble
    {
        Vector3 UiOffset { get;}
        
        Vector3 Pos{get;}

        string TextCanInteractAble { get; }
        string TextCannotInteractAble { get; }


        void StartInteract();
        void FinishedInteract();

        bool CanInteraction(PlayerController playerController);



        event EventHandler<IInteractAble> OnFinishedInteract;
    }
}
