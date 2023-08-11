using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.UserInterface;

namespace TPSHorror.Interaction
{
    public class UIInteractionManager : MonoBehaviour
    {
        private static UIInteractionManager m_Instance = null;
        public static UIInteractionManager Instance
        {
            get
            {
                return m_Instance;
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

        private void OnEnable()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
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

        //private void OnCurrentInteractStart(object sender, IInteractAble e)
        //{
        //    Debug.Log($"Interaction : {sender}[{e}] start");
        //    e.OnStartInteract -= OnCurrentInteractStart;
        //}

        //public void OnCurrentInteractionFinished(object sender, IInteractAble e)
        //{
        //    Debug.Log($"Interaction : {sender}[{e}] Finished");
        //    e.OnFinishedInteract -= OnCurrentInteractionFinished;
        //    CloseUIInteract();
        //}
    }
}
