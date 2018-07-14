using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.UI
{
    public class LoadingUI : MonoBehaviour
    {

        private Animator _titleAnimator;

        [SerializeField] float timer = 4.5f;
        [HideInInspector] public bool init = false;

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
            // Loading animations...

            init = true;
        }

        public IEnumerator Play()
        {
            // TODO Lanzar animaciones

            yield return new WaitForSeconds(timer);
        }

        public void Stop()
        {

        }


    } // Class
} // namespace