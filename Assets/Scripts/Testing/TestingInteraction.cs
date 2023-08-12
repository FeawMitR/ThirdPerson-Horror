using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;
using System;
using TPSHorror.PlayerControllerCharacter;

namespace TPSHorror
{
    public class TestingInteraction : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;

        public Vector3 UiOffset 
        {
            get 
            {
                return m_UIOffset;
            } 
        }

        public Vector3 Pos => UiOffset + this.transform.position;

        public string TextCanInteractAble => $"Press : ";

        public string TextCannotInteractAble => string.Empty;

        public event EventHandler<IInteractAble> OnStartInteract;
        public event EventHandler<IInteractAble> OnFinishedInteract;

        public void StartInteract()
        {
            OnStartInteract?.Invoke(this,this);
            Debug.Log($"Start Interact {this.gameObject.name}");
            FinishedInteract();
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this,this);
            Debug.Log($"Finished Interact {this.gameObject.name}");
            Destroy(this.gameObject);
        }

        public bool CanInteraction(PlayerController playerController)
        {
           return true;
        }
    }
}
