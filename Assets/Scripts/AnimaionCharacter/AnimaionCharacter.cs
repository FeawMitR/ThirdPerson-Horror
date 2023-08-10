using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorro.AnimaionCharacter
{
    [RequireComponent(typeof(Animator))]
    public class AnimaionCharacter : MonoBehaviour
    {
        private Animator m_Animator = null;

        private int m_AnimIDSpeed; 

        private const string m_Speed = "Speed";

        private void Awake()
        {
            m_Animator = this.GetComponent<Animator>();
            m_AnimIDSpeed = Animator.StringToHash(m_Speed);
        }

        public void UpdateSpeedMovementAnimation(float speed)
        {
            //Debug.LogError($"speed {speed}");
            m_Animator.SetFloat(m_AnimIDSpeed, speed);
        }
    }
}
