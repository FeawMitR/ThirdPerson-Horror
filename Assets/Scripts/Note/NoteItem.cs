using UnityEngine;
using TPSHorror.Interaction;
using System;
using TPSHorror.PlayerControllerCharacter;
using TPSHorror.UserInterface;

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

        [SerializeField]
        private bool m_IsReadingNote = false;

        [SerializeField]
        [TextArea]
        private string m_NoteString = string.Empty;


        public bool CanInteraction(PlayerController playerController)
        {
            return !m_IsReadingNote;
        }


        public void StartInteract()
        {
            UINoteManager.Instance.onUiNoteHideEvent += OnCloseUINote;
            UINoteManager.Instance.ShowUINote(m_NoteString);

            m_IsReadingNote = true;
            FinishedInteract();
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);  
        }
      

        private void OnCloseUINote()
        {
            UINoteManager.Instance.onUiNoteHideEvent -= OnCloseUINote;
            Debug.LogError($"OnCloseUINote");
            m_IsReadingNote = false;
        }
    }
}
