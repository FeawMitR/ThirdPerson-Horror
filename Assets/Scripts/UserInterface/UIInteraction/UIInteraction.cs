using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    using Text = TMPro.TextMeshProUGUI;
    public class UIInteraction : UICanvasGroup
    {
       
        private RectTransform m_RectTransform = null;

        protected override void Initialize()
        {
            base.Initialize();
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
            m_RectTransform = this.GetComponent<RectTransform>();
        }

        public void Show(Vector2 rectTranformPosition)
        {
            Show();
            m_RectTransform.anchoredPosition = rectTranformPosition;
        }
    }
}
