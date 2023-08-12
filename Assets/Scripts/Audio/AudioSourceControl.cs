using System;
using UnityEngine;

namespace TPSHorror.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceControl : MonoBehaviour
    {
        public enum AudioSourceState
        {
            None,Playing,Finished
        }

        private AudioSource m_AudioSource = null;
        private AudioSourceState m_State = AudioSourceState.None;
        private Transform m_Follow = null;

        public Action<AudioSourceControl> onPlayFinishedEvent;

        public bool IsPlay
        {
            get
            {
                return m_State == AudioSourceState.None;
            }
        }

        void Awake()
        {
            m_AudioSource = this.GetComponent<AudioSource>();
            m_State = AudioSourceState.None;
        }

        public void PlayAudio(AudioClip audio,bool isLoop, float volume = 1.0f,Transform follow = null)
        {
            if(m_State == AudioSourceState.Playing)
            {
                return;
            }

            if(follow != null)
            {
                m_Follow = follow;
                this.transform.SetParent(follow);
            }

            m_AudioSource.loop = isLoop;
            m_AudioSource.volume = volume;
            if (!isLoop)
            {
                m_AudioSource.PlayOneShot(audio);
            }
            else
            {
                m_AudioSource.clip = audio;
                m_AudioSource.Play();
            }

            m_State = AudioSourceState.Playing;
        }

        public void StopAudio()
        {
            PlayFinished();
         
        }

        private void PlayFinished()
        {
            m_State = AudioSourceState.None;
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }

            if (m_Follow != null)
            {
                m_Follow = null;
                this.transform.SetParent(null);
            }

            onPlayFinishedEvent?.Invoke(this);
        }

        // Update is called once per frame
        void Update()
        {
            switch (m_State)
            {
                case AudioSourceState.Playing:
                    if (!m_AudioSource.isPlaying)
                    {
                        PlayFinished();
                    }
                   
                    break;
            }
        }
    }
}
