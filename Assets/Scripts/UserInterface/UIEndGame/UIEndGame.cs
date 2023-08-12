using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    using Text = TMPro.TextMeshProUGUI;
    public class UIEndGame : UICanvasGroup
    {
        [SerializeField]
        private Text m_EndGameTypeText = null;

        public Text EndGameTypeText
        {
            get
            {
                return m_EndGameTypeText;
            }
        }

        [SerializeField]
        private Button m_ButtonReStartGame = null;

        public Button ButtonReStartGame
        {
            get
            {
                return m_ButtonReStartGame;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Hide();
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
