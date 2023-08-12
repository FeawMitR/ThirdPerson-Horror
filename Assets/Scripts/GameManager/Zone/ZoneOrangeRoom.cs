using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;
using TPSHorror.Item;

namespace TPSHorror.CoreManager.Zone
{
    public class ZoneOrangeRoom : ZoneManagement
    {
     
        [SerializeField]
        private Door m_Door_First = null;
        [SerializeField]
        private FuseBox m_FuseBox = null;
        [SerializeField]
        private Door m_Door_FuseBox = null;
        [SerializeField]
        private KeyItem m_FinalKey = null;

        public override void StartZone()
        {
            base.StartZone();

            m_Door_First.StartOperating();

            m_FuseBox.onFuseBoxHaveFuseEvent += OnFuseBoxHaveFuseEvent;

            m_FinalKey.OnFinishedInteract += OnFinishedInteractFinalKey;
        }

        private void OnFuseBoxHaveFuseEvent()
        {
            m_FuseBox.onFuseBoxHaveFuseEvent -= OnFuseBoxHaveFuseEvent;
            m_Door_FuseBox.StartOperating();
        }



        private void OnFinishedInteractFinalKey(object sender, IInteractAble e)
        {
            e.OnFinishedInteract -= OnFinishedInteractFinalKey;
            CollectedFinalKey();
        }

        private void CollectedFinalKey()
        {
            onZoneFinishedEvent?.Invoke();
        }
    }
}
