using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;
using System;
using TPSHorror.PlayerControllerCharacter;

namespace TPSHorror.Item
{
    public class KeyItem : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;

        public Vector3 UiOffset => m_UIOffset;

        public Vector3 Pos => this.transform.position + UiOffset;

        public event EventHandler<IInteractAble> OnFinishedInteract;

        public bool CanInteraction(PlayerController playerController)
        {
            return true;
        }

        public virtual void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);
            Destroy(this.gameObject);
        }

        public virtual void StartInteract()
        {
            FinishedInteract();
        }
    }
}
