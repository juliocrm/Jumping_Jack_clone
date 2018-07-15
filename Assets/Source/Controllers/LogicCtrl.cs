using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class LogicCtrl : MonoBehaviour
    {
        private int tick = 0;

        #region Singleton
        public static LogicCtrl Instance { get; private set; }

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

        // Update is called once per frame
        void Update()
        {

        }

        public void Tic()
        {
            
            if (tick == 4)
            {
                AvatarCtrl.Instance.Tic(tick);

                UpdateLogic();
                tick = 0;
            }
            tick++;
        }

        public void UpdateLogic()
        {
            // TODO Test Avatar contacts

            AvatarCtrl.Instance.Standing();
            if (InputMgr.LeftPressed){
                AvatarCtrl.Instance.RunLeft();
            }
            if (InputMgr.RightPressed) { 
                AvatarCtrl.Instance.RunRight();
            }
            if (InputMgr.JumpPressed)
            {
                AvatarCtrl.Instance.Jump();
            }                
        }

        public void ResetGame()
        {
            
        }

    } // Class
} // namespace