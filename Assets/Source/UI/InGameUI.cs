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
            scoreText.text = score.ToString();
        }

        public void SetMaxScore(int score)
        {
            maxScoreText.text = score.ToString();
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