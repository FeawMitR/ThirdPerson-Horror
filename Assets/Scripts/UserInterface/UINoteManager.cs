using System;
using UnityEngine;
using TPSHorror.CoreManager;

namespace TPSHorror.UserInterface
{
    public class UINoteManager : MonoBehaviour
    {
        private static UINoteManager instance = null;
        public static UINoteManager Instance
        {
            get
            {
                return instance;
            }
        }

        [SerializeField]
        private UiNote m_NoteUIPrefab = null;
        private UiNote m_NoteUI = null;

        public Action onUiNoteShowEvent;
        public Action onUiNoteHideEvent;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);

                Initialized();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            UnInitialized();
        }


        private void Initialized()
        {
            m_NoteUI = Instantiate(m_NoteUIPrefab, CanvasInstance.Instance.Canvas.transform);
            m_NoteUI.Hide();
            m_NoteUI.CloseButton.onClick.AddListener(CloseUIINote);
        }

        private void UnInitialized()
        {
            if (m_NoteUI)
            {
                m_NoteUI.CloseButton.onClick.RemoveListener(CloseUIINote);
                Destroy(m_NoteUI.gameObject);
            }
          
            Destroy(this.gameObject);
        }

        public void ShowUINote(string note)
        {
            GameManager.Instance.PauseGame();
        
            m_NoteUI.Show(note);
            onUiNoteShowEvent?.Invoke();
        }

        public void CloseUIINote()
        {
            GameManager.Instance.ResumeGame();
            m_NoteUI.Hide();
            onUiNoteHideEvent?.Invoke();
        }

    }
}
