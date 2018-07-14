using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.Managers
{
    public class AudioMgr : MonoBehaviour
    {


        #region Singleton
        public static AudioMgr Instance { get; private set; }

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

        // Update is called once per frame
        void Update()
        {

        }
    } // Class
} // namespace