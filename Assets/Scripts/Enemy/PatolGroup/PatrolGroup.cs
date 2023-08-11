using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.Enemy
{
    public class PatrolGroup : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_PatrolPath = null;

        public Vector3 GetPatrolPosition(int index,out int nextIndex)
        {
            if(index >= m_PatrolPath.Length)
            {
                nextIndex = 0;
                return m_PatrolPath[^1].position;
            }

            nextIndex =  index+1;
            return m_PatrolPath[index].position;
        }

#if UNITY_EDITOR

        [SerializeField]
        private Color gizmosColor = Color.white;
        private const float radius = 0.5f;
        private void OnDrawGizmos()
        {
            if(m_PatrolPath == null || m_PatrolPath.Length <= 0)
            {
                return;
            }

            Gizmos.color = gizmosColor;
            for (int i = 0; i < m_PatrolPath.Length; i++)
            {
                Gizmos.DrawWireSphere(m_PatrolPath[i].position, radius);
            }

          
            if(m_PatrolPath.Length > 0)
            {
                for (int i = 0; i < m_PatrolPath.Length - 1; i++)
                {
                    Gizmos.DrawLine(m_PatrolPath[i].position, m_PatrolPath[i + 1].position);
                }
                Gizmos.color = Color.red;
                Gizmos.DrawLine(m_PatrolPath[^1].position, m_PatrolPath[0].position);
            }
        }
#endif
    }
}
