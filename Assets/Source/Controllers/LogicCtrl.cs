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

        private int skipFallingTest = 0;

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
        
        public void PlayLevel()
        {
            logicState = State.Playing;
            GameMgr_JJ.Instance.PlayTics();
        }

        public void Tic()
        {
            tick++;
            //if (AvatarCtrl.Instance.actualState == AvatarCtrl.States.Standing)
            //    ApplyInput();
            

            HolesCtrl.Instance.Tic(tick);
            EnemiesCtrl.Instance.Tic(tick);

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
            if (TestFallingInHoles() == 1) {
                AvatarCtrl.Instance.Falling();
            }
            else if (TestFallingInHoles() == 2) {
                LifePointsCtrl.Instance.LoseLife();
                AvatarCtrl.Instance.Falling();
                
                if (LifePointsCtrl.Instance.Lifes == 0)
                    GameOver();
                return;
            }

            if (skipFallingTest == 0)
            {
                if (TestEnemyContact())
                {
                    AvatarCtrl.Instance.Kicked();
                    return;
                }
            }
            else
                skipFallingTest--;

            ApplyInput();
        }

        public int TestFallingInHoles()
        {
            if (!CheatsMgr.FallingEnable())
                return 0;

            if (HolesCtrl.Instance.ExistHoleDown(AvatarCtrl.Instance.cellPosition))
            {
                if (AvatarCtrl.Instance.cellPosition.y - 3 == 0)
                {
                    return 2; // Cae y pierde vida
                }
                else
                    return 1; // cayendo
            }
            else
                return 0; // No cae
        }

        public bool TestEnemyContact()
        {
            if (!CheatsMgr.EnemyContactEnabled())
                return false;

            return EnemiesCtrl.Instance.TestEnemyIn(AvatarCtrl.Instance.cellPosition);                
        }

        public int TestJump()
        {
            if (AvatarCtrl.Instance.cellPosition.y + 3 == 24)
                return 2; // Última línea

            if (CheatsMgr.AlwaysJumpOK())
                return 1;

            if (!HolesCtrl.Instance.ExistHoleUp(AvatarCtrl.Instance.cellPosition))
                return 0;
                                    
            return 1; // Salta a siguiente
        }

        private bool standing = true;
        private void ApplyInput()
        {
            if (InputMgr.LeftPressed)
            {
                standing = false;
                AvatarCtrl.Instance.RunLeft();
            }
            if (InputMgr.RightPressed)
            {
                standing = false;
                AvatarCtrl.Instance.RunRight();
            }
            if (InputMgr.JumpPressed)
            {
                standing = false;

                if (TestJump() == 0)
                    AvatarCtrl.Instance.BadJump();


                else if (TestJump() == 1)
                {
                    LifePointsCtrl.Instance.AddScore(5 * LevelMgr.Instance.ActualLevel);

                    skipFallingTest += 16;
                    HolesCtrl.Instance.AddHoles(1);
                    AvatarCtrl.Instance.Jump();
                }
                else if (TestJump() == 2)
                {
                    LifePointsCtrl.Instance.AddScore(5 * LevelMgr.Instance.ActualLevel);
                    // Avatar Last Jump Anim
                    skipFallingTest += 16;
                    HolesCtrl.Instance.AddHoles(1);
                    logicState = State.FinishingLevel;
                }
            }

            if (standing) {
                AvatarCtrl.Instance.Standing();
            }
            else
                standing = true;

        }

        public void LevelCompleted()
        {
            GameMgr_JJ.Instance.StopTics();

            if (actualLevel == 21)
            {
                logicState = State.GameOver;
                // Lanzar pantalla de Final Juego
                return;
            }

            logicState = State.ScoreScreen;

            LevelMgr.Instance.LevelCompleted();

            actualLevel++;
        }

        public void GameOver()
        {
            GameMgr_JJ.Instance.StopTics();
            
            logicState = State.GameOver;
            
            LevelMgr.Instance.GameOver();
        }
        
        public void ResetGame()
        {
            avatarLives = 6;
            actualLevel = 0;
        }

    } // Class
} // namespace