using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TPSHorror.PerceptionSystem
{
    public class PerceptionFieldOfViewSensor : PerceptionSensor
    {
        [SerializeField]
        private float m_ViewRadius = 5.0f;
        [SerializeField]
        private float m_ViewNearRadius = 2.0f;
        [SerializeField,Range(30.0f,90.0f)]
        private float m_ViewAngle = 90.0f;

        private bool m_IsIgnorViewNearRadius = false;

        [SerializeField]
        private LayerMask m_TargetMask;
        [SerializeField]
        private LayerMask m_ObstacleMask;



        public float ViewRadius
        {
            get
            {
                return m_ViewRadius;
            }
            set
            {
                m_ViewRadius = value;
            }
        }

        public float ViewNearRadius
        {
            get
            {
                return m_ViewNearRadius;
            }
            set
            {
                m_ViewNearRadius = value;
            }
        }

        public bool IsIgnorViewNearRadius
        {
            get
            {
                return m_IsIgnorViewNearRadius;
            }
            set
            {
                m_IsIgnorViewNearRadius = value;
            }
        }


        public override void SensorOperatingSign(out Transform target)
        {
            if (!m_IsOperating)
            {
                target = null;
                return;
            }

            target = FindVisibleTarget();
        }


        public Vector3 DirFromAngle(float angleInDegrees,bool isWorld)
        {
            if (!isWorld)
            {
                angleInDegrees += this.transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees*Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private Transform FindVisibleTarget()
        {

#if UNITY_EDITOR
            m_VisibleTarget.Clear();
#endif
            Collider[] targetsViewRadius = Physics.OverlapSphere(transform.position,m_ViewRadius, m_TargetMask);
            for (int i = 0; i < targetsViewRadius.Length; i++)
            {
                PerceptionFieldOfViewSign sign = targetsViewRadius[i].GetComponent<PerceptionFieldOfViewSign>();
                if (sign)
                {
                    Transform target = sign.SignTransform;
                    Vector3 direction = (target.position - transform.position).normalized;

                    float distance = Vector3.Distance(transform.position, target.position);

                    if(distance <= m_ViewNearRadius && !m_IsIgnorViewNearRadius)
                    {
                        if (!IsObstacleBlock(direction, distance))
                        {
#if UNITY_EDITOR
                            m_VisibleTarget.Add(target);
                            return target;
#endif
                        }
                    }
                    else
                    {
                        Vector3 newDirection = new Vector3(direction.x,0, direction.z);
                        if (Vector3.Angle(transform.forward, newDirection) < m_ViewAngle / 2)
                        {
                            if (!IsObstacleBlock(direction, distance))
                            {
#if UNITY_EDITOR
                                m_VisibleTarget.Add(target);
                                return target;
#endif
                            }
                        }
                    }


                }
            }

            return null;
        }

        public bool IsObstacleBlock(Vector3 direction,float distance)
        {
            return Physics.Raycast(transform.position, direction, distance, m_ObstacleMask);
        }


#if UNITY_EDITOR

        private List<Transform> m_VisibleTarget = new List<Transform>();

        private void OnDrawGizmos()
        {
            Handles.color = Color.yellow;
            Handles.DrawWireArc(this.gameObject.transform.position,Vector3.up,Vector3.forward, 360, m_ViewRadius);

            Handles.color = Color.red;
            Handles.DrawWireArc(this.gameObject.transform.position, Vector3.up, Vector3.forward, 360, ViewNearRadius);


            Vector3 viewAngleA = DirFromAngle(-m_ViewAngle / 2,false);
            Vector3 viewAngleB = DirFromAngle(m_ViewAngle / 2, false);

            Handles.DrawLine(transform.position, transform.position + viewAngleA * m_ViewRadius);
            Handles.DrawLine(transform.position, transform.position + viewAngleB * m_ViewRadius);

            if(m_VisibleTarget != null || m_VisibleTarget.Count > 0)
            {
                for (int i = 0; i < m_VisibleTarget.Count; i++)
                {
                    Handles.DrawLine(transform.position, m_VisibleTarget[i].position);
                }
            }
           
        }

#endif
    }
}
