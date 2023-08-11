using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Enemy;

namespace TPSHorror.GameManager.Zone
{
    public class ZoneManagement : MonoBehaviour
    {
        [System.Serializable]
        public struct EnemyInfo
        {
            public EnemyController m_Enemy;
            public PatrolGroup m_patrolGroup;
        }

        [SerializeField]
        private EnemyInfo[] m_Enemies;
     
        public void StartZone()
        {
            for (int i = 0; i < m_Enemies.Length; i++)
            {
                if (m_Enemies[i].m_Enemy && m_Enemies[i].m_patrolGroup)
                {
                    m_Enemies[i].m_Enemy.GroupPatrol = m_Enemies[i].m_patrolGroup;
                    m_Enemies[i].m_Enemy.StartOperation();
                }
            }
        }

        public void StopZone()
        {
            for (int i = 0; i < m_Enemies.Length; i++)
            {
                if (m_Enemies[i].m_Enemy)
                {
                    m_Enemies[i].m_Enemy.StopOperation();
                }
            }
        }
    }
}
