using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JumpingJack.UI;
using JumpingJack.Managers;

namespace JumpingJack.Controllers
{
    public class LifePointsCtrl : MonoBehaviour
    {

        public int Lifes { get; private set; }
        public int Score { get; private set; }
        public int MaxScore { get; private set; }

        #region Singleton
        public static LifePointsCtrl Instance { get; private set; }

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

        public void Init()
        {
            ResetData();
        }

        public void ResetData()
        {
            // Read MAX SCORE
            MaxScore = 0;
            Score = 0;
            Lifes = 0;
        }

        public void SetLives(int lives)
        {
            Lifes = lives;
        }

        public void AddScore(int score)
        {
            Score += score;
            InGameUI.Instance.SetScore(Score);
            if (Score > PersistenceMgr.MaxScore)
            {
                PersistenceMgr.SaveGame(Score);
                InGameUI.Instance.SetMaxScore(Score);
            }

        }

        public void AddLife()
        {
            Lifes++;
            InGameUI.Instance.SetLifes(Lifes);
        }

        public void LoseLife()
        {
            Lifes--;
            InGameUI.Instance.SetLifes(Lifes);
        }

    } // Class
} // namespace