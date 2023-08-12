using System.Collections;
using System.Collections.Generic;
using TPSHorror.Interaction;
using TPSHorror.Item;
using UnityEngine;

namespace TPSHorror.CoreManager.Zone
{
    public class ZoneYellowRoom : ZoneManagement
    {
        [SerializeField]
        private Door m_DoorZoneOne = null;

        [SerializeField]
        private KeyItem m_KeyOrangeRoom = null;

        public override void StartZone()
        {
            base.StartZone();
            m_DoorZoneOne.StartOperating();

            m_KeyOrangeRoom.OnFinishedInteract += OnFinishedCollectInteractKeyOrangeRoom;
        }

        private void OnFinishedCollectInteractKeyOrangeRoom(object sender, IInteractAble e)
        {
            e.OnFinishedInteract -= OnFinishedCollectInteractKeyOrangeRoom;
            onZoneFinishedEvent.Invoke();
        }
    }
}
