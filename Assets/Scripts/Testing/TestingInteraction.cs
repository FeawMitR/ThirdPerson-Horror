using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TPSHorror.Interaction;

namespace TPSHorror
{
    public class TestingInteraction : MonoBehaviour, IInteractAble
    {
        [SerializeField]
        private Vector3 m_UIOffset;

        public Vector3 UiOffset 
        {
            get 
            {
                return m_UIOffset;
            } 
        }

        public Vector3 Pos => UiOffset + this.transform.position;
    }
}
