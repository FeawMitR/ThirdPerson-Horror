using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.PerceptionSystem
{
    public abstract class PerceptionSensor : MonoBehaviour
    {
        protected bool m_IsOperating = false;
        public virtual void StartSensor()
        {
            m_IsOperating = true;
        }

        public virtual void StopSensor()
        {
            m_IsOperating = false;
        }

        public abstract void SensorOperatingSign(out Transform target);
    }
}
