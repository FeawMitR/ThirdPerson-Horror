using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;

namespace TPSHorror.GameManager.Zone
{
    public class ZoneOrangeRoom : ZoneManagement
    {
        //public struct
        [SerializeField]
        private Door m_Door_First = null;


        public override void StartZone()
        {
            base.StartZone();

            m_Door_First.StartOperating();
        }
    }
}
