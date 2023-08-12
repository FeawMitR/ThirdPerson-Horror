using System;
using System.Collections;
using System.Collections.Generic;
using TPSHorror.PlayerControllerCharacter;
using UnityEngine;

namespace TPSHorror.Interaction
{
    public class FuseItem : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;
        public Vector3 UiOffset => m_UIOffset;

        public Vector3 Pos => this.transform.position + UiOffset;

        [SerializeField]
        private string m_TextCanInteractAble;

        public string TextCanInteractAble => m_TextCanInteractAble;

        public string TextCannotInteractAble => string.Empty;

        [SerializeField]
        private MeshRenderer m_FuseMesh = null;

        //[SerializeField]
        private int m_FuseNumber = 0;
        public int FuseNumber
        {
            get
            {
                return m_FuseNumber;
            }
        }

        public event EventHandler<IInteractAble> OnFinishedInteract;

        public bool CanInteraction(PlayerController playerController)
        {
            return true;
        }

       
        public void SetFuse(int number,Color color)
        {
            m_FuseNumber = number;
            m_FuseMesh.material.SetColor("_BaseColor", color);
        }

        public void StartInteract()
        {
            FinishedInteract();
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);
        }
    }
}
