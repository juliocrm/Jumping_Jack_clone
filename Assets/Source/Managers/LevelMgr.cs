using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JumpingJack.Controllers;
using JumpingJack.Utilities;
using JumpingJack.UI;

namespace JumpingJack.Managers
{
    public class LevelMgr : MonoBehaviour
    {
        [SerializeField] private GameObject linesPrefab;

        public int ActualLevel { get; set; }

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
            LifePointsCtrl.Instance.ResetData();
            LifePointsCtrl.Instance.SetLives(6);
            InGameUI.Instance.SetLifes(LifePointsCtrl.Instance.Lifes);
            InGameUI.Instance.SetScore(0);
            InGameUI.Instance.SetMaxScore(PersistenceMgr.MaxScore);

            // TODO Dibujar elementos
            ActualLevel = 1;
            LogicCtrl.Instance.PlayLevel(0);
            
            AvatarCtrl.Instance.ResetAvatar();
        }

        public void LevelCompleted()
        {
            StartCoroutine(LevelCompletedCoroutine());
        }

        private IEnumerator LevelCompletedCoroutine()
        {
            EnemiesCtrl.Instance.ResetController();

            EndLevelUI.Instance.EnableScreen();
            EndLevelUI.Instance.StartScreen(ActualLevel, ActualLevel-1);

            yield return new WaitForSeconds(13);
            EndLevelUI.Instance.DisableScreen();

            PlayNextLevel();
        }

        private void PlayNextLevel()
        {
            ActualLevel++;
            EnemiesCtrl.Instance.Init(ActualLevel - 1);

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