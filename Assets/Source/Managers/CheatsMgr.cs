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