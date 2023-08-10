using UnityEngine;

namespace TPSHorror.UserInterface
{
    public abstract class UIBase : MonoBehaviour
    {

        private void Awake()
        {
            Initialize();
        }

        protected abstract void Initialize();

        public abstract void Show();

        public abstract void Hide();
    }
}
