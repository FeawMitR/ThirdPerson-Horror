using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TPSHorror.PerceptionSystem;

namespace TPSHorror
{
    [RequireComponent(typeof(NavMeshAgent))]
  
    public class EnemyController : MonoBehaviour
    {
        public enum EnemyState : byte
        {
            Patrolling,Hunting
        }
        private bool m_IsOperating = false;
        [SerializeField]
        private EnemyState m_EnemyState = EnemyState.Patrolling;

        private NavMeshAgent m_Agent = null;
        private bool m_IsMoving = false;

        [Header("Movement")]
        [SerializeField]
        private float m_WalkSpeed = 2.5f;

        [SerializeField]
        private float m_RunSpeed = 5.0f;


        [Header("Patrol")]
        [SerializeField]
        private PatrolGroup m_GroupPatrol = null;
        [SerializeField]
        private int m_CurrentPatrolIndex = 0;
        [SerializeField]
        private int m_NextPatrolIndex = 0;

        [Header("Perception Sensor")]
        [SerializeField]
        private Transform m_TargetHunting = null;
        [Header("Perception Sensor FieldOfView",order = 1)]
        [SerializeField]
        private PerceptionFieldOfViewSensor m_FieldOfViewSensorSensors = null;
        [SerializeField]
        private float m_FindNotHuntingRadius = 5.0f;
        [SerializeField]
        private float m_FindNotHuntingDangerRadius = 3.0f;


        private void Awake()
        {
            Initialized();
        }

        private void Start()
        {
            //ToDo : Remove
            StartOperation();
        }

        private void Initialized()
        {
            InitializedNavMeshAgent();
        }


        private void InitializedNavMeshAgent()
        {
            m_Agent = this.GetComponent<NavMeshAgent>();
            m_Agent.speed = m_WalkSpeed;
        }

        public void StartOperation()
        {
            m_IsOperating = true;
            StartPatrolState();
        }

        public void StopOperation()
        {
            m_IsOperating = false;
        }

        private void Update()
        {
            if (!m_IsOperating)
            {
                return;
            }

            switch (m_EnemyState)
            {
                case EnemyState.Patrolling:
                    UpdatePatrolling();
                    break;
            }
        }

        private bool IsPathComplete
        {
            get
            {
                if (!m_Agent.pathPending)
                {
                    if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    {
                        if (!m_Agent.hasPath || m_Agent.velocity.sqrMagnitude == 0f)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

    

        private void StartPatrolState()
        {
            m_EnemyState = EnemyState.Patrolling;
            m_Agent.speed = m_WalkSpeed;
            StartCurrentPatrol();
            StartFieldOfViewSensorSensorsNotHunting();
        }

        private void StopPatrolState()
        {
            m_Agent.SetDestination(this.transform.position);
        }

        private void StartCurrentPatrol()
        {
            Vector3 destiation = m_GroupPatrol.GetPatrolPosition(m_CurrentPatrolIndex,out m_NextPatrolIndex);

            m_Agent.SetDestination(destiation);
            m_IsMoving = true;
        }

        private void FinishedCurrentPatrol()
        {
            m_IsMoving = false;
            m_CurrentPatrolIndex = m_NextPatrolIndex;

            StartCurrentPatrol();
        }

        private void UpdatePatrolling()
        {
            if (IsPathComplete)
            {
                if (m_IsMoving)
                {
                    FinishedCurrentPatrol();
                }
            }
        }



        private void StartFieldOfViewSensorSensorsNotHunting()
        {
            m_FieldOfViewSensorSensors.ViewRadius = m_FindNotHuntingRadius;
            m_FieldOfViewSensorSensors.ViewNearRadius = m_FindNotHuntingDangerRadius;
            m_FieldOfViewSensorSensors.StartSensor();
            StartCoroutine(FindTargetNotHuntingState());
        }

        private void StopFieldOfViewSensorSensorsNotHunting()
        {
            m_FieldOfViewSensorSensors.StopSensor();
        }

        private IEnumerator FindTargetNotHuntingState()
        {
            while (m_TargetHunting == null)
            {
                yield return new WaitForSeconds(0.5f);
                m_FieldOfViewSensorSensors.SensorOperatingSign(out m_TargetHunting);
            }
            FoundTargetNotHunting();
        }

        private void FoundTargetNotHunting()
        {
            Debug.LogError($"Found Target : {m_TargetHunting.name}");
            StopFieldOfViewSensorSensorsNotHunting();
            if(m_EnemyState == EnemyState.Patrolling)
            {
                StopPatrolState();
            }
            StartHuntingState();
        }






        private void StartHuntingState()
        {
            m_EnemyState = EnemyState.Hunting;
        }
    }
}
