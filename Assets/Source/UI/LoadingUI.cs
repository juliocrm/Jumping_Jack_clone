using System.Collections;
using UnityEngine;

namespace JumpingJack.UI
{
    public class LoadingUI : MonoBehaviour
    {

        [SerializeField] private GameObject backgroundPanel;

        [SerializeField] LoadingScreenAnimation loadingScreenAnimation;

        [HideInInspector] public bool init = false;
        bool animationFinished = false;

        #region Singleton
        public static LoadingUI Instance { get; private set; }

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
            init = true;
        }

        public IEnumerator Play()
        {
            backgroundPanel.SetActive(true);
            loadingScreenAnimation.IntroAnimation();

            yield return new WaitWhile(() => !loadingScreenAnimation.AnimFinished);
        }
        
        public void Stop()
        {
            loadingScreenAnimation.OutAnimation();
            backgroundPanel.SetActive(false);
        }


    } // Class
} // namespace