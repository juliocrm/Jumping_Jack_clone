using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JumpingJack.Managers;

namespace JumpingJack.UI
{
    public class EndLevelUI : MonoBehaviour
    {
        [SerializeField] private Text nextLevelText;
        [SerializeField] private Text infoText;

        [SerializeField] private string[] infoTexts;

        [SerializeField] GameObject background;
        [SerializeField] private GameObject replayTExtParent;


        #region Singleton
        public static EndLevelUI Instance { get; private set; }

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
            NormalizeText(ref infoText, textSizeproportion);

            DisableScreen();
        }

        public void StartScreen(int nextEnemies, int infoIndex)
        {
            replayTExtParent.SetActive(false);
            if (nextEnemies == 1)
                nextLevelText.text = "NEXT LEVEL - " + nextEnemies.ToString() + " HAZARD";
            else
                nextLevelText.text = "NEXT LEVEL - " + nextEnemies.ToString() + " HAZARDS";

            StartCoroutine(AnimateText(infoIndex));
        }

        private IEnumerator AnimateText(int index)
        {
            
            string[] tempString = infoTexts[index].Split(',');
            string s = "";

            for (int i = 0; i < tempString.Length; i++)
            {
                if (tempString[i] == "\\n")
                {
                    s += "\n";
                }
                else
                    s += tempString[i];
                infoText.text = s;
                yield return new WaitForSeconds(0.1f);
            }

            if (LevelMgr.Instance.ActualLevel == 21)
                replayTExtParent.SetActive(true);
        }

        public float textSizeproportion;
        public RectTransform textBox;
        private void NormalizeText(ref Text text, float proportion)
        {
            int sizeText = Mathf.CeilToInt(textBox.rect.height * proportion);
            Debug.Log("Size: " + sizeText);
            text.fontSize = Mathf.CeilToInt(textBox.rect.height * proportion);
        }

        public void EnableScreen()
        {
            background.SetActive(true);
        }

        public void DisableScreen()
        {
            nextLevelText.text = "";
            infoText.text = "";
            background.SetActive(false);
        }

    } // Class
} // namespace