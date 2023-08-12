using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Item;
using TPSHorror.Interaction;

namespace TPSHorror.GameManager.Zone
{
    public class ZoneManagement_Main : ZoneManagement
    {
        [SerializeField]
        private KeyItem m_KeyYellowRoom = null;


        public override void StartZone()
        {
            base.StartZone();

            m_KeyYellowRoom.OnFinishedInteract += OnFinishedCollectInteract;
        }


        private void OnFinishedCollectInteract(object sender, IInteractAble e)
        {
            e.OnFinishedInteract -= OnFinishedCollectInteract;
            onZoneFinishedEvent.Invoke();
        }
    }
}
