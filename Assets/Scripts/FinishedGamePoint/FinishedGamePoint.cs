using System;
using UnityEngine;

namespace TPSHorror.FinishedGame
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class FinishedGamePoint : MonoBehaviour
    {

        private bool m_Operating = false;

        public Action onPlayerTriggerEvent;
     
        private void Start()
        {
            StartOperation();
            Collider col = this.GetComponent<Collider>();
            if (col)
            {
                col.isTrigger = true;
            }

            Rigidbody rigid = this.GetComponent<Rigidbody>();
            if (rigid)
            {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
            }
           
        }

        public void StartOperation()
        {
            m_Operating = true; 
        }

        public void StopOperation() 
        {
            m_Operating = false;
        }




        private void OnTriggerEnter(Collider other)
        {
            if (!m_Operating)
            {
                return;
            }

            if(other.tag == "Player")
            {
                StopOperation();
                onPlayerTriggerEvent?.Invoke();
            }
        }


    }
}
