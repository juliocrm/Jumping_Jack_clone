using System.Collections;
using UnityEngine;

using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class LogicCtrl : MonoBehaviour
    {
        private int tick = 0;
        private int avatarLives = 6;
        private int actualLevel = 0;

        // Comprueba si se ha presionado el botón de correr a
        // izq o der o jump, sino, permite ir al estado standing.
        private bool standing = true;

        // Cuneta unos últimos tics antes de mostrar la pantalla 
        // de presentación del siguiente nivel. Así permite la 
        // última animación de saltar del Avatar.
        private int finishLevelTics;

        private int gameOverLastTicks = 0;
        

        private enum State { Default,
            Paused,
            Playing,
            FinishingLevel,
            ScoreScreen,
            GameOver }

        private State logicState = State.Default;

        public enum FallingState{NotFalling, Falling, FallingDie}

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

        
        public void PlayLevel()
        {
            logicState = State.Playing;
            GameMgr_JJ.Instance.PlayTics();
        }

        public void Tic()
        {
            tick++;

            HolesCtrl.Instance.Tic(tick);
            EnemiesCtrl.Instance.Tic(tick);

            AvatarCtrl.Instance.Tic(tick);
            if (tick == 4)
            {
                if (logicState == State.FinishingLevel)
                {
                    if (finishLevelTics == 0)
                    {
                        AvatarCtrl.Instance.Jump();
                        AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.Win);
                    }

                    finishLevelTics++;
                    if (finishLevelTics == 2)
                    {
                        finishLevelTics = 0;
                        LevelCompleted();
                    }
                }
                if(logicState == State.GameOver)
                {
                    if(gameOverLastTicks == 8)
                    {
                        GameOver();
                        gameOverLastTicks = 0;
                    }
                    gameOverLastTicks++;
                }

                UpdateLogic();
                tick = 0;
            }
        }

        public void UpdateLogic()
        {
            if (TestFallingInHoles() == FallingState.Falling) {
                AvatarCtrl.Instance.Falling();
            }
            else if (TestFallingInHoles() == FallingState.FallingDie) {
                LifePointsCtrl.Instance.LoseLife();
                AvatarCtrl.Instance.Falling();

                if (LifePointsCtrl.Instance.Lifes == 0)
                {
                    logicState = State.GameOver;
                }
                    return;
            }
            
            if (TestEnemyContact())
            {
                Debug.Log("Enemy contact");
                AvatarCtrl.Instance.Kicked();
                return;
            }
            ApplyInput();
        }

        public FallingState TestFallingInHoles()
        {
            if (!CheatsMgr.FallingEnable())
                return FallingState.NotFalling;

            if (HolesCtrl.Instance.ExistHoleDown(AvatarCtrl.Instance.cellPosition))
            {
                if (AvatarCtrl.Instance.cellPosition.y - 3 == 0)
                {
                    return FallingState.FallingDie;
                }
                else
                    return FallingState.Falling;
            }
            else
                return FallingState.NotFalling;
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
            {
                if (AvatarCtrl.Instance.cellPosition.y == 0)
                    return 3; // pierde vida
                else
                    return 0;
            }
            
                                    
            return 1; // Salta a siguiente
        }

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
                int testJump = TestJump();
                if (testJump == 0)
                    AvatarCtrl.Instance.BadJump();


                else if (testJump == 1)
                {
                    LifePointsCtrl.Instance.AddScore(5 * LevelMgr.Instance.ActualLevel);

                    //skipFallingTest += 16;
                    HolesCtrl.Instance.AddHoles(1);
                    AvatarCtrl.Instance.Jump();
                }
                else if (testJump == 2)
                {
                    LifePointsCtrl.Instance.AddScore(5 * LevelMgr.Instance.ActualLevel);
                    // Avatar Last Jump Anim
                    //skipFallingTest += 16;
                    HolesCtrl.Instance.AddHoles(1);
                    logicState = State.FinishingLevel;
                }
                else if (testJump == 3)
                {
                    LifePointsCtrl.Instance.LoseLife();

                    AvatarCtrl.Instance.BadJump();

                    if (LifePointsCtrl.Instance.Lifes == 0)
                        logicState = State.GameOver;
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
            StartCoroutine(LevelCompleteCoroutine());
        }

        private IEnumerator LevelCompleteCoroutine()
        {
            GameMgr_JJ.Instance.StopTics();
            
            yield return new WaitForSeconds(2);

            

            if (actualLevel == 21)
            {
                logicState = State.GameOver;

                // Lanzar pantalla de Final Juego

            }
            else
            {
                logicState = State.ScoreScreen;

                LevelMgr.Instance.LevelCompleted();

                actualLevel++;
            }

        }

        public void GameOver()
        {
            AvatarCtrl.Instance.actualState = AvatarCtrl.States.other;
            AvatarCtrl.Instance.Standing();
            AudioMgr.Instance.StopSound();
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