//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.Managers
{
    public class AudioMgr : MonoBehaviour
    {
        [SerializeField] private AudioClip standing;
        [SerializeField] private AudioClip goodJump;
        [SerializeField] private AudioClip badJump;
        [SerializeField] private AudioClip running;
        [SerializeField] private AudioClip knockOut;
        [SerializeField] private AudioClip falling;
        [SerializeField] private AudioClip win;
        
        private AudioSource audioSource;

        public enum AudioFx { Standing, Running, GoodJump,
                                BadJump, KnockOut, Falling, Win}

        #region Singleton
        public static AudioMgr Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                if (Instance != this)
                {
                    DestroyImmediate(this);
                }
            }
            else
            {
                Instance = this;
            }
        }
        #endregion


        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFx(AudioFx fx)
        {
            switch(fx)
            {
                case AudioFx.BadJump:
                    audioSource.loop = false;
                    audioSource.clip = badJump;
                    break;
                case AudioFx.GoodJump:
                    audioSource.loop = false;
                    audioSource.clip = goodJump;
                    break;
                case AudioFx.Falling:
                    audioSource.loop = false;
                    audioSource.clip = falling;
                    break;
                case AudioFx.Running:
                    audioSource.loop = true;
                    audioSource.clip = running;
                    break;
                case AudioFx.KnockOut:
                    audioSource.loop = true;
                    audioSource.clip = knockOut;
                    break;
                case AudioFx.Standing:
                    audioSource.loop = true;
                    audioSource.clip = standing;
                    break;
                case AudioFx.Win:
                    audioSource.loop = false;
                    audioSource.clip = win;
                    break;
            }

            audioSource.Play();
        }
        public void StopSound()
        {
            audioSource.Stop();
        }

    } // Class
} // namespace