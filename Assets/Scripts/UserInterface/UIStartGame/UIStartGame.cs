using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    public class UIStartGame : UICanvasGroup
    {
        [SerializeField]
        private Button m_ButtonStartGame = null;
        public Button ButtonStartGame
        {
            get
            {
                return m_ButtonStartGame;
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
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;
        }
    }
}
