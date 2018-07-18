using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JumpingJack.UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text maxScoreText;
        [SerializeField] private Image[] lifeIcons;

        #region Singleton
        public static InGameUI Instance { get; private set; }

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

        public void SetScore(int score)
        {
            string s = "";
            FillString(ref s, 10000, score);
            s += score.ToString();
            scoreText.text = s;
        }

        public void SetMaxScore(int score)
        {
            string s = "";
            FillString(ref s, 10000, score);
            s += score.ToString();

            maxScoreText.text = s;
        }

        private void FillString(ref string s, int max, int value)
        {
            if(max > 1)
            if (value < max)
            {
                s += "0";
                FillString(ref s, max / 10, value);
            }
        }

        public void SetLifes(int lives)
        {
            for(int i = 0; i < lifeIcons.Length; i++)
            {
                if(i<lives)
                    lifeIcons[i].enabled = true;
                else
                    lifeIcons[i].enabled = false;
            }
        }



    } // Class
} // namespace