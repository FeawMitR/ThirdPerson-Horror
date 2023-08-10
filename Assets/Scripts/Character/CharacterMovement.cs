using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.Character
{
    [RequireComponent(typeof(CharacterController))]

    public class CharacterMovement : MonoBehaviour
    {
        private CharacterController m_characterController = null;

        private static float m_gravity = -9.81f;
        private Vector3 m_GravityDirection = Vector3.zero;


        private void Awake()
        {
            Initialized();
        }

        private void Initialized()
        {
            m_characterController = this.GetComponent<CharacterController>();
        }


        public void Move(Vector3 direction)
        {
            if (!m_characterController)
            {
                return;
            }

            m_characterController.Move(direction);
            GravityMovement();
        }

        public void Rotation(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }


        private void GravityMovement()
        {
            if (!m_characterController)
            {
                return;
            }

            if (!m_characterController.isGrounded)
            {
                m_GravityDirection.y += m_gravity * Time.deltaTime;
            }
            else
            {
                m_GravityDirection.y = 0;
            }

            m_characterController.Move(m_GravityDirection * Time.deltaTime);
        }

  
    }
}
