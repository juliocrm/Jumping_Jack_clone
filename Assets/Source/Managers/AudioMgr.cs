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

        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private AudioSource secondaryAudioSource;

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


        public void PlaySoundFx(AudioFx fx)
        {
            switch(fx)
            {
                case AudioFx.BadJump:
                    mainAudioSource.loop = false;
                    mainAudioSource.clip = badJump;
                    break;
                case AudioFx.GoodJump:
                    mainAudioSource.loop = false;
                    mainAudioSource.clip = goodJump;
                    break;
                case AudioFx.Falling:
                    mainAudioSource.loop = false;
                    mainAudioSource.clip = falling;
                    break;
                case AudioFx.Running:
                    mainAudioSource.loop = true;
                    mainAudioSource.clip = running;
                    break;
                case AudioFx.KnockOut:
                    mainAudioSource.loop = true;
                    mainAudioSource.clip = knockOut;
                    break;
                case AudioFx.Standing:
                    mainAudioSource.loop = true;
                    mainAudioSource.clip = standing;
                    break;
                case AudioFx.Win:
                    mainAudioSource.loop = false;
                    mainAudioSource.clip = win;
                    break;
            }

            mainAudioSource.Play();
        }

        public void PlayExtraSoundFx(AudioClip clip)
        {
            secondaryAudioSource.clip = clip;
            secondaryAudioSource.Play();
        }

        public void StopSound()
        {
            mainAudioSource.Stop();
        }

    } // Class
} // namespace