using UnityEngine;
using UnityEngine.UI;

namespace TPSHorror.UserInterface
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInstance : MonoBehaviour
    {
        private static CanvasInstance m_Instance = null;
        public static CanvasInstance Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private Canvas m_Canvas = null;
        public Canvas Canvas
        {
            get
            {
                return m_Canvas;
            }
        }

        private RectTransform m_CanvasRectTransform = null;

        public RectTransform CanvasRectTransform
        {
            get
            {
                return m_CanvasRectTransform;
            }
        }

        private void OnEnable()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                DontDestroyOnLoad(this.gameObject);
                m_Canvas = this.GetComponent<Canvas>();
                m_CanvasRectTransform = m_Canvas.GetComponent<RectTransform>();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
