using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.UI;

namespace JumpingJack.Managers
{
    public class GameMgr_JJ : MonoBehaviour {


        private float _tic = 0.087f;

        private bool init = false;
        
        public delegate void TIC();
        public static event TIC OnTic;

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
            // Start Tic
            var loadingCoroutine = StartCoroutine(LoadingUI.Instance.Play());

            var localInit = StartCoroutine(Init());

            yield return loadingCoroutine;
            yield return localInit;

            LoadingUI.Instance.Stop();
            
            LevelMgr.Instance.PlayNewGame();
        }

        // Update is called once per frame
        void Update() {

        }

        private IEnumerator Init()
        {
            StartCoroutine(TIC_Coroutine());

            PersistenceMgr.Instance.Init();
            yield return new WaitWhile(() => !PersistenceMgr.Instance.Loaded);

            LevelMgr.Instance.Init();
            
            init = true;
        }

        
        private IEnumerator TIC_Coroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_tic);
                if(OnTic != null)
                    OnTic();
            }
        }

        public void ResetGame()
        {
            // TODO No hay reset en el juego original pero en caso
            // de querer reiniciar todo el juego sería mejor tener esta
            // opción que reniciar la aplicación.
        }
        
    } // Class
} // namespace