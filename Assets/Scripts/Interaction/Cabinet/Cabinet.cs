using System;
using UnityEngine;
using TPSHorror.Interaction;
using TPSHorror.PlayerControllerCharacter;
using System.Collections;
using TPSHorror.Audio;

namespace TPSHorror
{
    public class Cabinet : MonoBehaviour, IInteractAble
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

        private bool m_IsFinished = false;

        [SerializeField]
        private Vector3 m_offsetTargetPush ;
        [SerializeField]
        private float m_Speed = 1.5f;
        [SerializeField]
        private AudioClip m_PushSFX = null;


        public bool CanInteraction(PlayerController playerController)
        {
   
            return !m_IsFinished;
        }

        public void StartInteract()
        {
            m_IsFinished = true;
            StartCoroutine(StartPushToTarget());
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);
        }


        private IEnumerator StartPushToTarget()
        {
            AudioManager.Instance.PlayAtWorldPosition(m_PushSFX, false, this.transform.position, 0.5f);
            Vector3 finalTarget = this.transform.position + m_offsetTargetPush;
            float time = 0.0f;

            while (time < 1)
            {
                time += Time.deltaTime;
                float progression = Mathf.Clamp01(time / (1 / m_Speed));
                this.transform.position = Vector3.Lerp(this.transform.position, finalTarget, progression);
                yield return null;
            }

            this.transform.position = finalTarget;
            FinishedInteract();
        }

    }
}
