using System;
using System.Collections;
using System.Collections.Generic;
using TPSHorror.PlayerControllerCharacter;
using UnityEngine;

namespace TPSHorror.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    public class Push : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;
        [SerializeField]
        private string m_TextCanInteractAble;

        public Vector3 UiOffset => m_UIOffset;

        public Vector3 Pos => transform.position + UiOffset;

        public string TextCanInteractAble => m_TextCanInteractAble;

        public string TextCannotInteractAble => string.Empty;

        public event EventHandler<IInteractAble> OnFinishedInteract;

        [SerializeField]
        private LayerMask m_LayerMaskPlayer;

        [SerializeField]
        private Rigidbody m_Rigid = null;
        private PlayerController m_Player = null;

        public bool CanInteraction(PlayerController playerController)
        {
            m_Player = playerController;
            return true;
        }

      

        public void StartInteract()
        {
            if (m_Player)
            {             
                Vector3 start = new Vector3(m_Player.transform.position.x, this.transform.position.y, m_Player.transform.position.z);
                Vector3 target = this.transform.position;

                Vector3 direction = (target - start).normalized;
                float distance = Vector3.Distance(start, target);

                if (Physics.Raycast(start, direction, out RaycastHit hit, distance, m_LayerMaskPlayer))
                {
                    //Debug.LogError($"{hit.collider}");
                    //Debug.DrawLine(start, start + direction * distance,Color.red,5.0f);

                    //Debug.DrawLine(target, target + direction * distance, Color.green, 5.0f);

                    m_Rigid.AddForce(direction * 20.0f,ForceMode.Impulse);
                }
            }
         
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);
            m_Player = null;
        }
    }
}
