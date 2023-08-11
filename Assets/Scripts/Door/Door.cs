using System;
using System.Collections;
using System.Collections.Generic;
using TPSHorror.Interaction;
using TPSHorror.PlayerControllerCharacter;
using UnityEngine;

namespace TPSHorror
{
    public class Door : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;

        [SerializeField]
        private Transform m_Knob = null;

        private bool m_IsOpen = false;
        private bool m_IsInteracting = false;

        [SerializeField]
        private float m_Speed = 30.0f;
        [SerializeField]
        private float m_StartRotation = 0.0f;
        [SerializeField]
        private float m_EndRotation = 90f;
        [SerializeField]
        private AnimationCurve m_Rotationcurve;


        //------ IInteractAble -----
        public Vector3 UiOffset => m_UIOffset;
        public Vector3 Pos => m_Knob.position + UiOffset;

        public event EventHandler<IInteractAble> OnStartInteract;
        public event EventHandler<IInteractAble> OnFinishedInteract;



 

        public void StartInteract()
        {
            m_IsInteracting = true;

            StopAllCoroutines();
            if (!m_IsOpen)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                StartCoroutine(CloseDoor());
            }
        }

        private IEnumerator OpenDoor()
        {
            Quaternion finalRotation = Quaternion.Euler(0, m_EndRotation, 0);
            float time = 0.0f;

            while(time < 1)
            {
                time += Time.deltaTime;
                float RotationProgression = Mathf.Clamp01(time / (1 / m_Speed));
                float RotationCurveValue = m_Rotationcurve.Evaluate(RotationProgression);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, finalRotation, RotationCurveValue);
                yield return null;
            }

            this.transform.rotation = finalRotation;
            FinishedInteract();
        }

        private IEnumerator CloseDoor()
        {
            Quaternion finalRotation = Quaternion.Euler(0, m_StartRotation, 0);
            float time = 0.0f;

            while (time <1)
            {
                time += Time.deltaTime;
                float RotationProgression = Mathf.Clamp01(time / (1 / m_Speed));
                float RotationCurveValue = m_Rotationcurve.Evaluate(RotationProgression);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, finalRotation, RotationCurveValue);
                yield return null;
            }

            this.transform.rotation = finalRotation;
            FinishedInteract();
        }


        public void FinishedInteract()
        {
            StopAllCoroutines();
            m_IsOpen = !m_IsOpen;
            m_IsInteracting = false;

            Debug.LogError($"FinishedInteract()");
            OnFinishedInteract?.Invoke(this,this);
        }

        public bool CanInteraction(PlayerController playerController)
        {
           return !m_IsInteracting;
        }
    }
}
