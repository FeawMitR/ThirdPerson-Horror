using System;
using TPSHorror.PlayerControllerCharacter;
using UnityEngine;

namespace TPSHorror.Interaction
{
    public class FuseBox : MonoBehaviour,IInteractAble
    {
        [System.Serializable]
        public struct FuseInfo
        {
            public MeshRenderer m_FuseMesh;
            public MeshRenderer m_FuseLight;
            public FuseItem m_FuseItem;

            public Color m_FuseColorDefault;
            public Color m_FuseColorActive;
        }

        [SerializeField]
        private FuseInfo[] m_Fuse = null;
        [SerializeField]
        private bool[] m_CollectedFuse = null;
        [SerializeField]
        private bool[] m_FilledFuse = null;

        [SerializeField]
        private Vector3 m_UIOffset;


        public event EventHandler<IInteractAble> OnFinishedInteract;

        public Vector3 UiOffset => m_UIOffset;

        public Vector3 Pos => this.transform.position + m_UIOffset;

        private const string m_InteractAble = "Put Fuse";
        public string TextCanInteractAble => TextCanInteractAbleFuseType;

        private const string m_InteractDontHaveFuse = "Find Fuse";
 
        public string TextCannotInteractAble => TextCannotInteractAbleFuseType;

        private bool m_IsFuseBoxHaveFuse = false;
        public Action onFuseBoxHaveFuseEvent;

        private void Start()
        {
            m_CollectedFuse = new bool[m_Fuse.Length];
            m_FilledFuse = new bool[m_Fuse.Length];
            for (int i = 0; i < m_Fuse.Length; i++)
            {
                if (m_Fuse[i].m_FuseMesh)
                {
                    m_Fuse[i].m_FuseMesh.material.SetColor("_BaseColor", m_Fuse[i].m_FuseColorDefault);
                }

                if (m_Fuse[i].m_FuseLight)
                {
                    m_Fuse[i].m_FuseLight.material.SetColor("_BaseColor", m_Fuse[i].m_FuseColorActive);
                }

                if (m_Fuse[i].m_FuseItem)
                {
                    m_Fuse[i].m_FuseItem.SetFuse(i,m_Fuse[i].m_FuseColorActive);
                    m_Fuse[i].m_FuseItem.OnFinishedInteract += CollectFuse;
                }
            }
            m_IsFuseBoxHaveFuse = false;
        }

        private void CollectFuse(object sender, IInteractAble e)
        {
            e.OnFinishedInteract -= CollectFuse;
            FuseItem fuse = e as FuseItem;
            if (fuse)
            {
                int number = fuse.FuseNumber;
                m_CollectedFuse[number] = true;
             
                fuse.gameObject.SetActive(false);
            }
        }

        public void StartInteract()
        {
            int putFuseNumber = PutFuse;
            m_FilledFuse[putFuseNumber] = true;
            if (m_Fuse[putFuseNumber].m_FuseMesh)
            {
                m_Fuse[putFuseNumber].m_FuseMesh.material.SetColor("_BaseColor", m_Fuse[putFuseNumber].m_FuseColorActive);
            }

            FinishedInteract();
        }

        public void FinishedInteract()
        {
            OnFinishedInteract?.Invoke(this, this);

            if (IsFuseBoxHaveFuse && !m_IsFuseBoxHaveFuse)
            {
                m_IsFuseBoxHaveFuse = true;
                onFuseBoxHaveFuseEvent?.Invoke();
            }
        }

        public bool CanInteraction(PlayerController playerController)
        {

            for (int i = 0; i < m_Fuse.Length; i++)
            {
                if (m_CollectedFuse[i] && !m_FilledFuse[i])
                {
                    return true;
                }
            }
            return false;
        }

        private string TextCanInteractAbleFuseType
        {
            get
            {
                for (int i = 0; i < m_Fuse.Length; i++)
                {
                    if (m_CollectedFuse[i] && !m_FilledFuse[i])
                    {
                        return $"{m_InteractAble} {i +1 }" ;
                    }
                }

                return string.Empty;
            }
        }

        private int PutFuse
        {
            get
            {
                for (int i = 0; i < m_Fuse.Length; i++)
                {
                    if (m_CollectedFuse[i] && !m_FilledFuse[i])
                    {
                        return i;
                    }
                }

                return -1;
            }
        }


        private string TextCannotInteractAbleFuseType
        {
            get
            {
                for (int i = 0; i < m_Fuse.Length; i++)
                {
                    if (!m_CollectedFuse[i] && !m_FilledFuse[i])
                    {
                        return $"{m_InteractDontHaveFuse} {i + 1}";
                    }
                }

                return string.Empty;
            }
        }

        public bool IsFuseBoxHaveFuse
        {
            get
            {
                for (int i = 0; i < m_Fuse.Length; i++)
                {
                    if (!m_CollectedFuse[i] && !m_FilledFuse[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
