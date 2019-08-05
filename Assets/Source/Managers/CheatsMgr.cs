using UnityEngine;

namespace JumpingJack.Managers
{
    public class CheatsMgr : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Preione 2 - Desactive para evitar el daño por contacto con enemigos.")]
        private bool enemyContact = true;

        [SerializeField]
        [Tooltip("Presione 1 - Desactive para caer por agujeros.")]
        private bool falling = true;

        [SerializeField]
        [Tooltip("Preseione 3 - Active para saltar en cualquier lugar.")]
        private bool alwaysJumping = false;

        [SerializeField]
        [Tooltip("Presione 4 - Slow motion")]
        private bool slowTime = false;

        [SerializeField]
        [Tooltip("Tiempo en segundos para activar cheat.")]
        private int TimeToEnableCheat = 5;

        private const float slowTimeScale = 0.3f;

        [SerializeField]
        private AudioClip cheatEnableClip;

        #region Singleton
        public static CheatsMgr Instance { get; private set; }

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
        
        private float alpha1Timer = 0;
        private float alpha2Timer = 0;
        private float alpha3Timer = 0;
        private float alpha4Timer = 0;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                if (alpha1Timer == 0)
                    alpha1Timer = Time.unscaledTime;
                
                if ((Time.unscaledTime - alpha1Timer) > TimeToEnableCheat)
                {
                    alpha1Timer = Time.unscaledTime;
                    falling = !falling;
                    AudioMgr.Instance.PlayExtraSoundFx(cheatEnableClip);
                }
            }
            else
                alpha1Timer = 0;

            if (Input.GetKey(KeyCode.Alpha2))
            {
                if (alpha2Timer == 0)
                    alpha2Timer = Time.unscaledTime;

                if ((Time.unscaledTime - alpha2Timer) > TimeToEnableCheat)
                {
                    alpha2Timer = Time.unscaledTime;
                    enemyContact = !enemyContact;
                    AudioMgr.Instance.PlayExtraSoundFx(cheatEnableClip);
                }
            }
            else
                alpha2Timer = 0;

            if (Input.GetKey(KeyCode.Alpha3))
            {
                if (alpha3Timer == 0)
                    alpha3Timer = Time.unscaledTime;

                if ((Time.unscaledTime - alpha3Timer) > TimeToEnableCheat)
                {
                    alpha3Timer = Time.unscaledTime;
                    alwaysJumping = !alwaysJumping;
                    AudioMgr.Instance.PlayExtraSoundFx(cheatEnableClip);
                }
            }
            else
                alpha3Timer = 0;

            if (Input.GetKey(KeyCode.Alpha4))
            {
                if (alpha4Timer == 0)
                    alpha4Timer = Time.unscaledTime;

                if ((Time.unscaledTime - alpha4Timer) > TimeToEnableCheat)
                {
                    alpha4Timer = Time.unscaledTime;
                    Time.timeScale = 0.3f;
                    slowTime = !slowTime;
                    Time.timeScale = slowTime ? slowTimeScale : 1;
                    AudioMgr.Instance.PlayExtraSoundFx(cheatEnableClip);
                }
            }
            else
                alpha4Timer = 0;
        }

        public static bool EnemyContactEnabled()
        {
            return Instance.enemyContact;
        }

        public static bool FallingEnable()
        {
            return Instance.falling;
        }

        public static bool AlwaysJumpOK()
        {
            return Instance.alwaysJumping;
        }

    } // Class
} // namespace