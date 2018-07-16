using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JumpingJack.Controllers;

namespace JumpingJack.Managers
{
    public class LevelMgr : MonoBehaviour
    {

        #region Singleton
        public static LevelMgr Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null)
            {
                if(Instance != this)
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

        public void Init()
        {
            AvatarCtrl.Instance.SetInitialPos(new Vector2(14,0));
            HolesCtrl.Instance.Init();

            GameMgr_JJ.OnTic += Tic;

        }

        public void PlayNewGame()
        {
            Debug.Log("Playing game");
            // TODO Dibujar elementos
            LogicCtrl.Instance.PlayLevel(0);
            
            AvatarCtrl.Instance.ResetAvatar();
        }

        private void Tic()
        {
            LogicCtrl.Instance.Tic();
        }

        private void OnDestroy()
        {
            GameMgr_JJ.OnTic -= Tic;
        }

    } // Class
} // namespace