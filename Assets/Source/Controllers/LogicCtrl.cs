﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class LogicCtrl : MonoBehaviour
    {
        private int tick = 0;
        private int avatarLives = 6;
        private int actualLevel = 0;

        private enum State{ Default,
                            Paused,
                            Playing,
                            FinishingLevel,
                            ScoreScreen,
                            GameOver}

        private State logicState = State.Default;


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

        public void PlayLevel(int level)
        {
            GameMgr_JJ.Instance.PlayTics();
        }

        public void Tic()
        {
            tick++;
            //if (AvatarCtrl.Instance.actualState == AvatarCtrl.States.Standing)
            //    ApplyInput();
                        
            AvatarCtrl.Instance.Tic(tick);

            if (tick == 4)
            {
                if (logicState == State.FinishingLevel)
                    LevelCompleted();

                UpdateLogic();
                tick = 0;
            }
        }

        public void UpdateLogic()
        {
            if(TestFallingInHoles() == 1) {
                AvatarCtrl.Instance.Falling();
            }
            else if (TestFallingInHoles() == 2){
                AvatarCtrl.Instance.Falling();
                avatarLives--;
                if (avatarLives == 0)
                    GameOver();
                return;
            }

            if (TestEnemyContact())
                AvatarCtrl.Instance.Kicked();
            else
                ApplyInput();
        }

        public int TestFallingInHoles()
        {
            return 0; // No cae
            //return 1; // Cayendo
            //if (AvatarCtrl.Instance.cellPosition.y - 3 == 0)
            //    return 2; // Cayendo en última línea
            

        }

        public bool TestEnemyContact()
        {
            return false;
        }

        public int TestJump()
        {
            if (AvatarCtrl.Instance.cellPosition.y + 3 == 24)
                return 2; // Última línea
            // return 0; // Incorrecto
            return 1; // Salta a siguiente
        }

        private void ApplyInput()
        {
            AvatarCtrl.Instance.Standing();
            if (InputMgr.LeftPressed)
            {
                AvatarCtrl.Instance.RunLeft();
            }
            if (InputMgr.RightPressed)
            {
                AvatarCtrl.Instance.RunRight();
            }
            if (InputMgr.JumpPressed)
            {
                if (TestJump() == 0)
                    AvatarCtrl.Instance.BadJump();

                else if(TestJump() == 1)
                    AvatarCtrl.Instance.Jump();

                else if (TestJump() == 2)
                {
                    // Avatar Last Jump Anim
                    logicState = State.FinishingLevel;
                }

            }
        }

        public void LevelCompleted()
        {
            GameMgr_JJ.Instance.StopTics();

            if (actualLevel == 19)
            {
                logicState = State.GameOver;
                // Lanzar pantalla de GameOver
                return;
            }

            logicState = State.ScoreScreen;
            // Lanzar pantalla de Puntuación
            
            Debug.Log("Level complete");
            actualLevel++;
        }

        public void GameOver()
        {

        }
        
        public void ResetGame()
        {
            avatarLives = 6;
            actualLevel = 0;
        }

    } // Class
} // namespace