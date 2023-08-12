using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace TPSHorror.UserInterface
{
    using Text = TMPro.TextMeshProUGUI;
    public class UIInteraction : UICanvasGroup
    {
       
        private RectTransform m_RectTransform = null;
        [SerializeField]
        private Text m_InteractText = null;
        private string key;

        protected override void Initialize()
        {
            base.Initialize();
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
            m_RectTransform = this.GetComponent<RectTransform>();
        }

        public void Show(Vector2 rectTranformPosition,string keyName)
        {
            Show();
            m_RectTransform.anchoredPosition = rectTranformPosition;
   
            m_InteractText.text = keyName;
            if (key != keyName)
            {
                key = keyName;
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_RectTransform);
            }
        }
    }
}
