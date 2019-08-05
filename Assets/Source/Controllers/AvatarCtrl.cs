using UnityEngine;
using JumpingJack.Utilities;

using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class AvatarCtrl : MonoBehaviour
    {
        public Transform primaryTransform;
        public const string stateName = "State";

        [HideInInspector] public Animator primaryAnimator;
        
        private Vector2 _InitialPosition = Vector2.zero;
        [HideInInspector] public Vector2 cellPosition;
        [HideInInspector] public Transform _transform;
        
        private float actualKickedFrames = 0;

        public enum States {    Standing,
                                RunningRight,
                                RunningLeft,
                                Jumping,
                                Falling,
                                Kicked,
                                BadJump,
                                KnockOut,
                                other}

        public States actualState = States.Standing;


        #region Singleton
        public static AvatarCtrl Instance { get; private set; }

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
                
        #region Creando estados
        private AvatarStateBase avatarState;
        private AvatarRunnigRight runningRightState = new AvatarRunnigRight();
        private AvatarRunningLeft runningLeftState = new AvatarRunningLeft();
        private AvatarJumping jumpingState = new AvatarJumping();
        private AvatarStanding standingState = new AvatarStanding();
        private AvatarKicked kickedState = new AvatarKicked();
        private AvatarBadJump badJumpState = new AvatarBadJump();
        private AvatarFalling fallingState = new AvatarFalling();
        private AvatarKnockOut knockOutState = new AvatarKnockOut();
        #endregion

        // Use this for initialization
        void Start()
        {
            primaryAnimator = primaryTransform.GetComponent<Animator>();

            _transform = GetComponent<Transform>();
            avatarState = standingState;
        }

        public void AddKickedFrames(int frames)
        {
            actualKickedFrames += frames;
        }

        public void Tic(int frame)
        {
            avatarState.Tic(frame);
            if(actualKickedFrames > 0)
            {
                actualKickedFrames--;
                if (actualKickedFrames <= 0)
                {
                    Debug.Log("Kicked Finished");
                    Standing();
                }
            }
        }

        #region Asignación de estados

        public void RunLeft()
        {
            if (actualState == States.RunningLeft)
                return;

            if (actualState == States.RunningRight
                || actualState == States.Standing)
            {
                AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.Running);

                avatarState = runningLeftState;
                actualState = States.RunningLeft;

                primaryAnimator.SetInteger(stateName, (int)States.RunningLeft);
            }
        }

        public void RunRight()
        {
            if (actualState == States.RunningRight)
                return;

            if (actualState == States.RunningLeft
                || actualState == States.Standing)
            {
                AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.Running);

                avatarState = runningRightState;
                actualState = States.RunningRight;

                primaryAnimator.SetInteger(stateName, (int)States.RunningRight);
            }
        }

        public void Jump()
        {
            if (actualState == States.RunningRight
                || actualState == States.RunningLeft
                || actualState == States.Standing)
            {
                AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.GoodJump);
                
                avatarState = jumpingState;
                actualState = States.Jumping;

                primaryAnimator.SetInteger(stateName, (int)States.Jumping);
            }
        }

        public void Standing()
        {
            if (actualState == States.Standing)
                return;

            if (actualState == States.Jumping)
                return;
            //if (actualState == States.Kicked)
            //    return;
            if (actualState == States.Falling)
                return;
            if (actualState == States.BadJump)
                return;
            //if (actualState == States.KnockOut)
            //    return;
            if (actualKickedFrames != 0)
                return;

            AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.Standing);
            
            avatarState = standingState;
            actualState = States.Standing;

            primaryAnimator.SetInteger(stateName, (int)States.Standing);
        }

        public void Kicked()
        {
            //if (actualState == States.KnockOut)
            //    return;

            avatarState = kickedState;
            actualState = States.Kicked;

            primaryAnimator.SetInteger(stateName, (int)States.KnockOut);
        }

        public void Falling()
        {
            AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.Falling);

            avatarState = fallingState;
            actualState = States.Falling;

            primaryAnimator.SetInteger(stateName, (int)States.Falling);
        }

        public void BadJump()
        {
            if (actualState == States.KnockOut)
                return;

            AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.BadJump);
            
            avatarState = badJumpState;
            actualState = States.BadJump;

            primaryAnimator.SetInteger(stateName, (int)States.BadJump);
        }

        public void KnockOut()
        {
            AudioMgr.Instance.PlaySoundFx(AudioMgr.AudioFx.KnockOut);


            avatarState = knockOutState;
            actualState = States.KnockOut;
        }

        #endregion

        public void ResetAvatar()
        {
            Instance.actualState = States.other;
            Instance.Standing();

            GameMgr_JJ.Instance.MultiplyTickDelay(1);

            cellPosition = _InitialPosition;
            SetSize();
            _transform.position = GameScreenCoords.CellToWorld(14,0);
            actualKickedFrames = 0;
        }

        public void SetInitialPos(Vector2 position)
        {
            _InitialPosition = position;
        }

        private void SetSize()
        {
            transform.localScale = new Vector3(GameScreenCoords.Units,
                                                GameScreenCoords.Units,1);
        }


    } // Class

    public class AvatarStateBase
    {
        virtual public void Tic(int frame) {
        }
    }

    public class AvatarRunningLeft : AvatarStateBase
    {
        Vector3 tempV3;
        public override void Tic(int frame)
        {
            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frame-1) / 3.0f));

            tempV3= GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
            tempV3.x -= GameScreenCoords.subUnit * (frame-1) ;
            AvatarCtrl.Instance._transform.position = tempV3;
            
            if (frame == 4)
            {
                if (AvatarCtrl.Instance.cellPosition.x - 1 == -1)
                {
                    AvatarCtrl.Instance.cellPosition.x = 30;
                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
                }
                else
                {
                    AvatarCtrl.Instance.cellPosition.x -= 1;

                }
            }
        }
    }

    public class AvatarRunnigRight : AvatarStateBase
    {
        Vector3 tempV3;
        public override void Tic(int frame)
        {


            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frame - 1) / 3.0f));

            tempV3 = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
            tempV3.x += GameScreenCoords.subUnit * (frame - 1);
            AvatarCtrl.Instance._transform.position = tempV3;

            if (frame == 4)
            {

                if (AvatarCtrl.Instance.cellPosition.x + 1 == 31)
                {
                    AvatarCtrl.Instance.cellPosition.x = 0;
                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
                }
                else
                {
                    AvatarCtrl.Instance.cellPosition.x += 1;

                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
                }
            }
        }
    }

    public class AvatarJumping : AvatarStateBase
    {
        private int frameCount = 0;
        public override void Tic(int frame)
        {
            if (frame == 1)
            {
                AvatarCtrl.Instance.primaryAnimator.SetInteger(AvatarCtrl.stateName, (int)AvatarCtrl.States.Jumping);
                GameMgr_JJ.Instance.MultiplyTickDelay(10f);
            }

            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frameCount) / 7.0f));
            frameCount++;

            if (frame == 2)
                GameMgr_JJ.Instance.MultiplyTickDelay(5f);
            else if(frame == 5)
                GameMgr_JJ.Instance.MultiplyTickDelay(18f);

            //if (frame == 4)
            //{
            if (frameCount == 8)
                {
                    frameCount = 0;

                    GameMgr_JJ.Instance.MultiplyTickDelay(1f);

                    AvatarCtrl.Instance.cellPosition.y += 3;

                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);

                    AvatarCtrl.Instance.actualState = AvatarCtrl.States.other;
                    AvatarCtrl.Instance.Standing();

                }
                //else
                //{
                //}
            //}
        }
    }

    public class AvatarStanding : AvatarStateBase
    {
        int frameCount = 0;
        public override void Tic(int frame)
        {
            if (frame == 1)
            {
                AvatarCtrl.Instance.primaryAnimator.SetInteger(AvatarCtrl.stateName, (int)AvatarCtrl.States.Standing);
            }
            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frameCount) / 95.0f));
            frameCount++;
            if (frameCount == 96)
                frameCount = 0;

        }
    }

    public class AvatarFalling : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if (frame == 1)
            {
                AvatarCtrl.Instance.primaryAnimator.SetInteger(AvatarCtrl.stateName, (int)AvatarCtrl.States.Falling);
                GameMgr_JJ.Instance.MultiplyTickDelay(10f);


                AvatarCtrl.Instance.AddKickedFrames(40); // 2.93sec
            }

            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frame -1) / 3.0f));

            if (frame == 4)
            {
                AvatarCtrl.Instance.cellPosition.y -= 3;
                GameMgr_JJ.Instance.MultiplyTickDelay(1f);

                AvatarCtrl.Instance.KnockOut();
                AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
            }
        }
    }

    public class AvatarKicked : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if(frame == 1)
                AvatarCtrl.Instance.AddKickedFrames(40); //1.91sec
            if (frame == 4)
                AvatarCtrl.Instance.KnockOut();
        }
    }

    public class AvatarBadJump : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frame - 1) / 3.0f));
            if (frame == 1)
            {
                
                AvatarCtrl.Instance.AddKickedFrames(60); // 3.05sec
            }
            if (frame == 2)
            {
                GameMgr_JJ.Instance.MultiplyTickDelay(30);
            }

            if (frame == 4)
            {
                GameMgr_JJ.Instance.MultiplyTickDelay(1);
                AvatarCtrl.Instance.KnockOut();
            }
        }
    }

    public class AvatarKnockOut : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if (frame == 1)
            {
                AvatarCtrl.Instance.primaryAnimator.SetInteger(AvatarCtrl.stateName, (int)AvatarCtrl.States.KnockOut);
            }

            AvatarCtrl.Instance.primaryAnimator.Play(0, 0, ((frame - 1) / 3.0f));
        }
    }

} // namespace