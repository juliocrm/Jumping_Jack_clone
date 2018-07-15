using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JumpingJack.Utilities;

namespace JumpingJack.Controllers
{
    public class AvatarCtrl : MonoBehaviour
    {

        private Vector2 _InitialPosition = Vector2.zero;
        [HideInInspector] public Vector2 cellPosition;
        [HideInInspector] public Transform _transform;

        private enum States {   Standing,
                                RunningRight,
                                RunningLeft,
                                Jumping,
                                Falling,
                                Kicked}


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



        // Use this for initialization
        void Start()
        {
            _transform = GetComponent<Transform>();
            avatarState = standingState;
        }

        public void Tic(int frame)
        {
            avatarState.Tic(frame);
        }

        #region Asignación de estados

        public void RunLeft()
        {
            avatarState = runningLeftState;
        }

        public void RunRight()
        {

            avatarState = runningRightState;
        }

        public void Jump()
        {
            avatarState = jumpingState;
        }

        public void Standing()
        {
            avatarState = standingState;
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

        private AvatarStateBase avatarState;
        private AvatarRunnigRight runningRightState = new AvatarRunnigRight();
        private AvatarRunningLeft runningLeftState = new AvatarRunningLeft();
        private AvatarJumping jumpingState = new AvatarJumping();
        private AvatarStanding standingState = new AvatarStanding();

    } // Class

    public class AvatarStateBase
    {
        virtual public void Tic(int frame) {
            Debug.Log("Aquí no debe llegar");
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
                    AvatarCtrl.Instance.cellPosition.y += 3;

                    AvatarCtrl.Instance._transform.position = GameScreenCoords.CellToWorld(AvatarCtrl.Instance.cellPosition);
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

} // namespace