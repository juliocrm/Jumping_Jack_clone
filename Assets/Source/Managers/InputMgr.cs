//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.Managers
{
    public class InputMgr : MonoBehaviour
    {

        public bool RightPressed { get; private set; }
        public bool LeftPressed  { get; private set; }
        public bool JumpPressed  { get; private set; }


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
            RightPressed = Input.GetKey(KeyCode.RightArrow);
            LeftPressed = Input.GetKey(KeyCode.LeftArrow);
            JumpPressed = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        }
    }
}