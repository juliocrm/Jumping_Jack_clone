using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.UI;

namespace JumpingJack.Managers
{
    public class GameMgr_JJ : MonoBehaviour {
        
        private enum States {   Starting,
                                Playing,
                                Paused,}
        private States state = States.Starting;

        [SerializeField] private float _tic = 0.087f;
        private float actualTic;

        private bool init = false;
        
        public delegate void TIC();
        public static event TIC OnTic;
        private Coroutine ticCoroutine;

        #region Singleton 
        public static GameMgr_JJ Instance { get; private set; }

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
        private IEnumerator Start() {
            yield return new WaitForSeconds(0.1f);

            actualTic = _tic;

            var loadingCoroutine = StartCoroutine(LoadingUI.Instance.Play());
            
            var localInit = StartCoroutine(Init());
            
            yield return loadingCoroutine;
            yield return localInit;

            LoadingUI.Instance.Stop();

            PlayGame();
        }

        // Update is called once per frame
        void Update() {

        }

        private IEnumerator Init()
        {
            PersistenceMgr.Instance.Init();
            yield return new WaitWhile(() => !PersistenceMgr.Instance.Loaded);

            LevelMgr.Instance.Init();
            
            init = true;
        }

        
        private IEnumerator TIC_Coroutine()
        {
            while (true)
            {
                if (state == States.Playing)
                {
                    if (OnTic != null)
                        OnTic();
                }
                yield return new WaitForSeconds(actualTic);
            }
        }

        public void MultiplyGameSpeed(float multiplier)
        {
            actualTic = _tic * multiplier;
        }

        public void PlayGame()
        {
            LevelMgr.Instance.PlayNewGame();
            state = States.Playing;
        }

        public void PauseGame()
        {
            state = States.Paused;
        }

        public void StopTics()
        {
            StopCoroutine(ticCoroutine);
        }

        public void PlayTics()
        {
            ticCoroutine = StartCoroutine(TIC_Coroutine());
        }

        public void ResetGame()
        {
            // TODO No hay reset en el juego original pero en caso
            // de querer reiniciar todo el juego sería mejor tener esta
            // opción que reniciar la aplicación.
        }
        
    } // Class
} // namespace