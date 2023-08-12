using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;
using System;
using TPSHorror.PlayerControllerCharacter;

namespace TPSHorror
{
    public class NoteItem : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;
        public Vector3 UiOffset => m_UIOffset;

        public Vector3 Pos => transform.position + UiOffset;

        public string TextCanInteractAble => "Read";

        public string TextCannotInteractAble => string.Empty;

        public event EventHandler<IInteractAble> OnFinishedInteract;

        public bool CanInteraction(PlayerController playerController)
        {
            return true;
        }

        void Start()
        {

        }
        public void StartInteract()
        {
            
        }

   


        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);
        }

      

      
    }
}
