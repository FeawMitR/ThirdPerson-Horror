using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.CutScene
{
    public class EventCutSceneManager : MonoBehaviour
    {

        private static EventCutSceneManager instance = null;
        public static EventCutSceneManager Instance
        {
            get
            {
                return instance;
            }
        }

        void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void OnCutScenePlay()
        {
            Time.timeScale = 0;
        }

        public void OnCutSceneFinished()
        {
            Time.timeScale = 1.0f;
        }
    }
}
