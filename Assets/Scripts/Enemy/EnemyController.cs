using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TPSHorror.PerceptionSystem;

namespace TPSHorror.Enemy
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
        public PatrolGroup GroupPatrol
        {
            set
            {
                m_GroupPatrol = value;
            }
        }
        [SerializeField]
        private int m_CurrentPatrolIndex = 0;
        [SerializeField]
        private int m_NextPatrolIndex = 0;

        [Header("Perception Sensor")]
        [SerializeField]
        private Transform m_TargetHunting = null;
        [SerializeField]
        private PerceptionFieldOfViewSensor m_FieldOfViewSensorSensors = null;

        [Header("Perception Sensor FieldOfView Not Hunting",order = 1)]
        [SerializeField]
        private float m_FindNotHuntingRadius = 5.0f;
        [SerializeField]
        private float m_FindNotHuntingDangerRadius = 3.0f;
        [Header("Perception Sensor FieldOfView Hunting", order = 1)]
        [SerializeField]
        private float m_FindHuntingRadius = 10.0f;
  
        [SerializeField]
        private float m_HuntingDangerRadius = 2.0f;

        private void Awake()
        {
            Initialized();
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

            switch (m_EnemyState)
            {
                case EnemyState.Patrolling:
                    StopPatrolState();
                    break;

                case EnemyState.Hunting:
                    StopHuntingState();
                    break;
            }
            StopAllCoroutines();
        }

        private void Update()
        {
            if (!m_IsOperating)
            {
                return;
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

        #region Patrol

        private void StartPatrolState()
        {
            Debug.Log($"StartPatrolState");
            m_EnemyState = EnemyState.Patrolling;
            m_Agent.speed = m_WalkSpeed;
            m_Agent.acceleration = m_WalkSpeed * 2;
            m_Agent.stoppingDistance = 0;

            StopAllCoroutines();

            StartCurrentPatrol();
            StartFieldOfViewSensorSensorsNotHunting();
        }

        private void StopPatrolState()
        {
            Debug.Log($"StopPatrolState");
            m_Agent.SetDestination(this.transform.position);
        }

        private void StartCurrentPatrol()
        {
            Vector3 destiation = m_GroupPatrol.GetPatrolPosition(m_CurrentPatrolIndex,out m_NextPatrolIndex);

            m_Agent.SetDestination(destiation);
            m_IsMoving = true;
            StartCoroutine(CheckPatrollingState());
        }

        private void FinishedCurrentPatrol()
        {
            m_IsMoving = false;
            m_CurrentPatrolIndex = m_NextPatrolIndex;

            StartCurrentPatrol();
        }


        private IEnumerator CheckPatrollingState()
        {
            while (!IsPathComplete)
            {
                yield return 0;
            }

            float randomDelay = Random.Range(0.5f,1.2f);
            yield return new WaitForSeconds(randomDelay);
            if (m_IsMoving)
            {
                FinishedCurrentPatrol();
            }
        }
     
        #endregion Patrol


        private void StartFieldOfViewSensorSensorsNotHunting()
        {
            m_FieldOfViewSensorSensors.ViewRadius = m_FindNotHuntingRadius;
            m_FieldOfViewSensorSensors.ViewNearRadius = m_FindNotHuntingDangerRadius;
            m_FieldOfViewSensorSensors.IsIgnorViewNearRadius = false;
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
            //Debug.LogError($"Found Target : {m_TargetHunting.name}");
            StopFieldOfViewSensorSensorsNotHunting();
            if(m_EnemyState == EnemyState.Patrolling)
            {
                StopPatrolState();
            }

            StartHuntingState();
        }



        #region Hunting


        private void StartHuntingState()
        {
            Debug.Log($"StartHuntingState");
            m_EnemyState = EnemyState.Hunting;
            m_Agent.speed = m_RunSpeed;
            m_Agent.acceleration = m_RunSpeed * 2;
            m_Agent.stoppingDistance = m_HuntingDangerRadius;

            StopAllCoroutines();

            StartCoroutine(FindTargetHuntingState());            
        }

        private void StopHuntingState()
        {
            Debug.Log($"StopHuntingState");
            m_TargetHunting = null;
            m_Agent.SetDestination(this.transform.position);
        }


        private IEnumerator FindTargetHuntingState()
        {
            Vector3 target = m_TargetHunting.position;
            m_Agent.SetDestination(target);
            m_IsMoving = true;
            Vector3 direction = (target - this.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
           
            while (m_TargetHunting != null && !IsPathComplete)
            {
                yield return new WaitForSeconds(0.5f);
                float distance = Vector3.Distance(transform.position, m_TargetHunting.position);
    
                if (m_TargetHunting)
                {
                    target = m_TargetHunting.position;
                }

                if (distance >= m_FindHuntingRadius)
                {
                    m_TargetHunting = null;
                }
                m_Agent.SetDestination(target);
              
                yield return 0;
            }

            m_IsMoving = false;
            if (m_TargetHunting == null)
            {
                TargetOutOfLength();
            }
            else
            {
                if (IsPathComplete)
                {
                    Vector3 directionToTarget = (m_TargetHunting.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, m_TargetHunting.position);
                    if (!m_FieldOfViewSensorSensors.IsObstacleBlock(directionToTarget, distance))
                    {
                        //m_TargetHunting.GetComponent<PlayerControllerCharacter.PlayerController>().PlayerWasCaught();
                        PlayerControllerCharacter.PlayerController player = m_TargetHunting.GetComponentInParent<PlayerControllerCharacter.PlayerController>();
                        player.PlayerWasCaught();
                        //TODO Fix
                        //StopHuntingState();
                        StopOperation();
                       
                        //Debug.LogError($"End Game");
                    }
                    else
                    {
                        m_TargetHunting = null;
                        TargetOutOfLength();

                    }
                }
               
            } 
        }


        //Stop Go To Patrol
        private void TargetOutOfLength()
        {
            StopHuntingState();
            StartPatrolState();
        }
        #endregion Hunting
    }
}
