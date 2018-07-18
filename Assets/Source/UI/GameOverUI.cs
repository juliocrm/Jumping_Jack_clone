using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JumpingJack.Managers;

namespace JumpingJack.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        [SerializeField] private Text enemiesText;
        [SerializeField] private GameObject newHiScoreGO;
        [SerializeField] private GameObject replayTextGO;
        [SerializeField] private GameObject gameTitleParent;
        [SerializeField] private GameObject pointsParent;
        [SerializeField] private GameObject parentGO;
        
        #region Singleton
        public static GameOverUI Instance { get; private set; }

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
            CloseScreen();
        }

        public void StartScreen(int score, int enemies, bool newHiScore)
        {
            Debug.Log("Score: " + score);
            parentGO.SetActive(true);
            string s = "";
            FillString(ref s, 10000, score);
            s += score;
            scoreText.text = s;

            enemiesText.text = enemies.ToString();
            StartCoroutine(StartScreenCoroutine(newHiScore));
        }

        private void FillString(ref string s, int max, int value)
        {
            if (max > 1)
                if (value < max)
                {
                    s += "0";
                    FillString(ref s, max / 10, value);
                }
        }

        private IEnumerator StartScreenCoroutine(bool newHiScore)
        {
            gameTitleParent.SetActive(true);
            yield return new WaitForSeconds(0.7f);

            pointsParent.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            
            if (newHiScore)
            {
                newHiScoreGO.SetActive(true);
                yield return new WaitForSeconds(0.7f);
            }

            replayTextGO.SetActive(true);
            StartCoroutine(ReadEnterCoroutina());
        }

        public void CloseScreen()
        {

            gameTitleParent.SetActive(false);
            pointsParent.SetActive(false);
            newHiScoreGO.SetActive(false);
            replayTextGO.SetActive(false);

            parentGO.SetActive(false);
        }

        private IEnumerator ReadEnterCoroutina()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (InputMgr.EnterPressed)
                {
                    GameMgr_JJ.Instance.PlayNewGame();
                    CloseScreen();
                    break;
                }
            }
        }

    } // Class
} // namespace