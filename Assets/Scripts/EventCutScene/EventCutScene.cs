using UnityEngine;
using UnityEngine.Playables;

namespace TPSHorror.CutScene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class EventCutScene : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector m_PlayableDirector = null;




        private void Start()
        {
            m_PlayableDirector.stopped += OnDirectorStopped;
            m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
        }


        public void StartCutscene()
        {
            m_PlayableDirector.Play();
            EventCutSceneManager.Instance.OnCutScenePlay();
        }

        public virtual void FinishedCutScene()
        {
            EventCutSceneManager.Instance.OnCutSceneFinished();
        }

        protected void OnDirectorStopped(PlayableDirector playable)
        {
            Debug.Log($"Finished");
            FinishedCutScene();
        }
    }
}
