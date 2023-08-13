using TPSHorror.Interaction;
using UnityEngine;

namespace TPSHorror.UserInterface
{
    public class UIInteractionManager : MonoBehaviour
    {
        private static UIInteractionManager instance = null;
        public static UIInteractionManager Instance
        {
            get
            {
                return instance;
            }
        }

        [SerializeField]
        private UIInteraction m_UiInteractionPrefab = null;
        private UIInteraction m_UiInteraction = null;

        private IInteractAble m_CurrentInteractAble = null;

        public IInteractAble CurrentInteractAble
        {
            get
            {
                return m_CurrentInteractAble;
            }
        }

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
            m_UiInteraction = Instantiate(m_UiInteractionPrefab,CanvasInstance.Instance.Canvas.transform);
            m_UiInteraction.Hide();
        }

        private void UnInitialized()
        {
            if (m_UiInteraction)
            {
                Destroy(m_UiInteraction.gameObject);
            }
           
            Destroy(this.gameObject);
        }

        public void ShowUIInteract(IInteractAble interactAble,string nameKey)
        { 
            m_CurrentInteractAble = interactAble;
           
            m_UiInteraction.Show(CanvasExtention.WorldPositionToCanvasPosition(CanvasInstance.Instance.CanvasRectTransform,Camera.main, interactAble.Pos), nameKey);
        }

        public void CloseUIInteract()
        {
            if(m_CurrentInteractAble == null)
            {
                return;
            }

            m_CurrentInteractAble = null;
            m_UiInteraction.Hide();
        }

    }
}
