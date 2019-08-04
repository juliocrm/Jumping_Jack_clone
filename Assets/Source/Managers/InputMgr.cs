using UnityEngine;

namespace JumpingJack.Managers
{
    public class InputMgr : MonoBehaviour
    {
        public static bool RightPressed { get; private set; }
        public static bool LeftPressed  { get; private set; }
        public static bool JumpPressed  { get; private set; }
        public static bool EnterPressed { get; private set; }

        public static InputMgr Instance { get; private set; }

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
        
        // Use this for initialization
        void Start()
        {
            RightPressed = false;
            LeftPressed = false;
            JumpPressed = false;
        }

        // Update is called once per frame
        void Update()
        {
            EnterPressed = Input.GetAxis("Submit") > 0;
            RightPressed = Input.GetAxis("Horizontal") > 0;
            LeftPressed  = Input.GetAxis("Horizontal") < 0;
            JumpPressed = Input.GetAxis("Jump") > 0;
        }
    }
}