﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JumpingJack.Controllers;
using JumpingJack.Utilities;

namespace JumpingJack.Managers
{
    public class LevelMgr : MonoBehaviour
    {
        [SerializeField] private GameObject linesPrefab;

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
            EnemiesCtrl.Instance.Init(1);

            GameMgr_JJ.OnTic += Tic;

        }

        public void PlayNewGame()
        {
            GenerateLines();
            Debug.Log("Playing game");
            // TODO Dibujar elementos
            LogicCtrl.Instance.PlayLevel(0);
            
            AvatarCtrl.Instance.ResetAvatar();
        }

        private void Tic()
        {
            LogicCtrl.Instance.Tic();
        }

        private Transform[] lines = new Transform[8];
        private void GenerateLines()
        {
            for(int i = 0; i < 8; i++)
            {
                lines[i] = Instantiate(linesPrefab).transform;
                lines[i].localScale = new Vector3(  GameScreenCoords.Units * 16f,
                                                    GameScreenCoords.Units * 1,1);
                lines[i].position = GameScreenCoords.CellToWorld(0, (i * 3) + 3);
            }
        }

        private void OnDestroy()
        {
            GameMgr_JJ.OnTic -= Tic;
        }

    } // Class
} // namespace