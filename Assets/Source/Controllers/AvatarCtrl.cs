//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using JumpingJack.Utilities;

using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class AvatarCtrl : MonoBehaviour
    {

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
                                KnockOut}

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
            if(actualKickedFrames != 0)
            {
                actualKickedFrames--;
                if (actualKickedFrames == 0)
                {
                    actualState = States.Standing;
                    Standing();
                }
            }
        }

        #region Asignación de estados

        public void RunLeft()
        {
            if (actualState == States.RunningRight
                || actualState == States.Standing)
            {
                avatarState = runningLeftState;
                actualState = States.RunningLeft;
            }
        }

        public void RunRight()
        {
            if (actualState == States.RunningLeft
                || actualState == States.Standing)
            {
                avatarState = runningRightState;
                actualState = States.RunningRight;
            }
        }

        public void Jump()
        {
            if (actualState == States.RunningRight
                || actualState == States.RunningLeft
                || actualState == States.Standing)
            {
                avatarState = jumpingState;
                actualState = States.Jumping;
            }
        }

        public void Standing()
        {
            if (actualState == States.Jumping)
                return;
            if (actualState == States.Kicked)
                return;
            if (actualState == States.Falling)
                return;
            if (actualState == States.BadJump)
                return;
            if (actualState == States.KnockOut)
                return;

            avatarState = standingState;
            actualState = States.Standing;
        }

        public void Kicked()
        {
            if (actualState == States.KnockOut)
                return;

            avatarState = kickedState;
            actualState = States.Kicked;
        }

        public void Falling()
        {
            if (actualState == States.KnockOut)
                return;

            avatarState = fallingState;
            actualState = States.Falling;
        }

        public void BadJump()
        {
            if (actualState == States.KnockOut)
                return;

            avatarState = badJumpState;
            actualState = States.BadJump;
        }

        public void KnockOut()
        {
            avatarState = knockOutState;
            actualState = States.KnockOut;
        }

        #endregion

        public void ResetAvatar()
        {
            cellPosition = _InitialPosition;

            _transform.position = GameScreenCoords.CellToWorld(14,0);
        }

        public void SetInitialPos(Vector2 position)
        {
            _InitialPosition = position;
        }


    } // Class

    public class AvatarStateBase
    {
        virtual public void Tic(int frame) {
        }
    }

    public class AvatarRunningLeft : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if(frame == 1) {
                // Determinar si esta en el borde
                // StartAnim
            }
            if(frame == 4)
            {
                if (AvatarCtrl.Instance.cellPosition.x - 1 == -1)
                {
                    AvatarCtrl.Instance.cellPosition.x = 30;
                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
                }
                else
                {
                    AvatarCtrl.Instance.cellPosition.x -= 1;

                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
                }
            }
        }
    }

    public class AvatarRunnigRight : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if(frame == 1)
            {
                // Determinar si esta en el borde
            }
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
                GameMgr_JJ.Instance.MultiplyGameSpeed(2);
            }
            if (frame == 4)
            {
                frameCount += frame;
                if (frameCount == 8)
                {
                    frameCount = 0;
                }
                else
                {
                    GameMgr_JJ.Instance.MultiplyGameSpeed(1f);

                    AvatarCtrl.Instance.cellPosition.y += 3;

                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);

                    AvatarCtrl.Instance.actualState = AvatarCtrl.States.Standing;
                }
            }
        }
    }

    public class AvatarStanding : AvatarStateBase
    {
        public override void Tic(int frame)
        {

        }
    }

    public class AvatarFalling : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if(frame == 1)
                AvatarCtrl.Instance.AddKickedFrames(55); // 2.93sec
            if (frame == 4)
                AvatarCtrl.Instance.KnockOut();
        }
    }

    public class AvatarKicked : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if(frame == 1)
                AvatarCtrl.Instance.AddKickedFrames(65); //1.91sec
            if (frame == 4)
                AvatarCtrl.Instance.KnockOut();
        }
    }

    public class AvatarBadJump : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            if (frame == 1)
            {
                AvatarCtrl.Instance.AddKickedFrames(115); // 3.05sec
            }
            if (frame == 4)
            {
                AvatarCtrl.Instance.KnockOut();
            }
        }
    }

    public class AvatarKnockOut : AvatarStateBase
    {
        public override void Tic(int frame)
        {
            // KnockOut Animation...
        }
    }

} // namespace