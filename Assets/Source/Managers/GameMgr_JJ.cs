using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.UI;
using JumpingJack.Controllers;
using JumpingJack.Utilities;

namespace JumpingJack.Managers
{
    public class GameMgr_JJ : MonoBehaviour {
        
        [SerializeField] private float _tic = 0.087f;
        [SerializeField] private int lifes = 6;

        private enum States {   Starting,
                                Playing,
                                Paused,}
        private States state = States.Starting;

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
#if UNITY_EDITOR
            var localInit = StartCoroutine(Init());
#else

            var loadingCoroutine = StartCoroutine(LoadingUI.Instance.Play());
            
            var localInit = StartCoroutine(Init());
            
            yield return loadingCoroutine;
#endif
            yield return localInit;
#if UNITY_EDITOR
#else
            LoadingUI.Instance.Stop();
#endif

            PlayNewGame();
        }

        // Update is called once per frame
        void Update() {

        }

        private IEnumerator Init()
        {
            yield return new WaitWhile(() => GameScreenCoords.UnitsReady == false);
            PersistenceMgr.Instance.Init();
            yield return new WaitWhile(() => PersistenceMgr.Instance.Loaded == false);
            
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

        public void MultiplyTickDelay(float multiplier)
        {
            actualTic = _tic * multiplier;
        }

        public void PlayNewGame()
        {
            LifePointsCtrl.Instance.ResetData();
            LifePointsCtrl.Instance.SetLives(lifes);
            LevelMgr.Instance.PlayNewGame();
            state = States.Playing;
        }

        public void PauseGame(bool pause)
        {
            if (pause)
                state = States.Paused;
            else
                state = States.Playing;
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