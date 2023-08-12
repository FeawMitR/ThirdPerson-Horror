using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance = null;
        public static AudioManager Instance
        {
            get
            {
                return instance;
            }
        }

        [SerializeField]
        private AudioSourceControl m_SourceControlPrefab = null;
        private Stack<AudioSourceControl> m_SourceControlPool = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;

                m_SourceControlPool = new Stack<AudioSourceControl>();
            }
            else
            {
                Destroy(this.gameObject);
            }

            
        }


        private AudioSourceControl InstantiateAudioSourceControl
        {
            get
            {
                return Instantiate(m_SourceControlPrefab);
            }
        }

        private AudioSourceControl GetAudioSourceControl
        {
            get
            {
                AudioSourceControl sourceControl = null;
                if (m_SourceControlPool.Count > 0)
                {
                    sourceControl = m_SourceControlPool.Pop();
                }

                if (sourceControl == null)
                {
                    sourceControl = InstantiateAudioSourceControl;
                }

                return sourceControl;
            }
        }

        public AudioSourceControl PlayAudio(AudioClip audioClip,bool isLoop,float volume = 1.0f,Transform follow = null)
        {
            AudioSourceControl sourceControl = GetAudioSourceControl;
            sourceControl.onPlayFinishedEvent += OnAudioSourceControlFinished;
            sourceControl.PlayAudio(audioClip, isLoop, volume,follow);
            return sourceControl;
        }

        public AudioSourceControl PlayAtWorldPosition(AudioClip audioClip, bool isLoop,Vector3 position, float volume = 1.0f)
        {
            AudioSourceControl sourceControl = GetAudioSourceControl;
            sourceControl.transform.position = position;
            sourceControl.onPlayFinishedEvent += OnAudioSourceControlFinished;
            sourceControl.PlayAudio(audioClip,isLoop, volume);
            return sourceControl;
        }

        private void OnAudioSourceControlFinished(AudioSourceControl sourceControl)
        {
            sourceControl.onPlayFinishedEvent -= OnAudioSourceControlFinished;
            m_SourceControlPool.Push(sourceControl);
        }
    }
}
