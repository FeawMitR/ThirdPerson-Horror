using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.CutScene
{
    public class EventCutSceneTriggerPoint : EventCutScene
    {

        private void OnTriggerEnter(Collider other)
        {


            if (other.tag == "Player")
            {
                StartCutscene();
                Debug.LogError($"SS");
            }
        }

        public override void FinishedCutScene()
        {
            base.FinishedCutScene();
            this.gameObject.SetActive(false);
        }
    }
}
