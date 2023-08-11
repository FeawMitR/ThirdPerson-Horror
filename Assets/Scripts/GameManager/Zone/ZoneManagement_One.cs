using System.Collections;
using System.Collections.Generic;
using TPSHorror.Interaction;
using UnityEngine;


namespace TPSHorror.GameManager.Zone
{
    public class ZoneManagement_One : ZoneManagement
    {
        [SerializeField]
        private Door m_DoorZoneOne = null;

        public override void StartZone()
        {
            base.StartZone();
            m_DoorZoneOne.StartOperating();
        }
    }
}
