using UnityEngine;

namespace JumpingJack.Managers
{
    public class CheatsMgr : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Desactive para evitar el daño por contacto con enemigos.")]
        private bool enemyContact = true;

        [SerializeField]
        [Tooltip("Desactive para caer por agujeros.")]
        private bool falling = true;

        [SerializeField]
        [Tooltip("Active para saltar en cualquier lugar.")]
        private bool alwaysJumping = false;
        
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

        // Use this for initialization
        void Start()
        {

        }

        private int alpha1Timer = 0;
        private int alpha2Timer = 0;
        private int alpha3Timer = 0;
        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                alpha1Timer++;
                if (alpha1Timer == 180)
                {
                    falling = !falling;
                    AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.GoodJump);
                }
            }
            else
                alpha1Timer = 0;

            if (Input.GetKey(KeyCode.Alpha2))
            {
                alpha2Timer++;
                if (alpha2Timer == 180)
                {
                    enemyContact = !enemyContact;
                    AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.GoodJump);
                }
            }
            else
                alpha2Timer = 0;

            if (Input.GetKey(KeyCode.Alpha3))
            {
                alpha3Timer++;
                if (alpha3Timer == 180)
                {
                    alwaysJumping = !alwaysJumping;
                    AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.GoodJump);
                }
            }
            else
                alpha3Timer = 0;

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