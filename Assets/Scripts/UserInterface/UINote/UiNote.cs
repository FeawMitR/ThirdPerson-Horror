using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    using Text = TMPro.TextMeshProUGUI;
    public class UiNote : UICanvasGroup
    {
        [SerializeField]
        private Button m_CloseButton = null;
        public Button CloseButton
        {
            get
            {
                return m_CloseButton;
            }
        }

        [SerializeField]
        private Text m_NoteText = null;
        
        public Text NoteText
        {
            get
            {
                return m_NoteText;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Hide();
        }


        public void Show(string note)
        {
            m_NoteText.text = note;
            Show();
        }

        public override void Show()
        {
            base.Show();
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;
        }

        public override void Hide()
        {
            base.Hide();
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }
    }
}
