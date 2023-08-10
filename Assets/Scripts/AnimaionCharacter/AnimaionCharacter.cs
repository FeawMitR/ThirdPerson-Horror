using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorro.AnimaionCharacter
{
    //[RequireComponent(typeof(Animator))]
    public class AnimaionCharacter : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Animator = null;

        private int m_AnimIDSpeed;
        private int m_AnimIDCrouched;

        private const string m_Speed = "Speed";
        private const string m_IsCrouched = "IsCrouched";

        private void Awake()
        {
            //m_Animator = this.GetComponent<Animator>();
            m_AnimIDSpeed = Animator.StringToHash(m_Speed);
            m_AnimIDCrouched = Animator.StringToHash(m_IsCrouched);
        }

        public void UpdateSpeedMovementAnimation(float speed)
        {
            //Debug.LogError($"speed {speed}");
            m_Animator.SetFloat(m_AnimIDSpeed, speed);
        }

        public void UpdateIsCrouchedAnimation(bool isCrouched)
        {
            m_Animator.SetBool(m_AnimIDCrouched,isCrouched);
        }
    }
}
