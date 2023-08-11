using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.PerceptionSystem
{
    public class PerceptionFieldOfViewSign : PerceptionSign
    {

        private Transform m_SignTransform = null;

        public Transform SignTransform
        {
            get
            {
                return m_SignTransform;
            }
            set
            {
                m_SignTransform = value;
            }
        }

        public override Vector3 SignPositon => throw new System.NotImplementedException();
    }
}
