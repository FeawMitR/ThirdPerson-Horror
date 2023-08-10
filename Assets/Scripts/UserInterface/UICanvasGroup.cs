using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UICanvasGroup : UIBase
    {
        protected CanvasGroup m_CanvasGroup = null;

        protected override void Initialize()
        {
            m_CanvasGroup = this.GetComponent<CanvasGroup>();
        }

        public override void Hide()
        {
            m_CanvasGroup.alpha = 0;
        }

        public override void Show()
        {
            m_CanvasGroup.alpha = 1;
        }
    }
}
