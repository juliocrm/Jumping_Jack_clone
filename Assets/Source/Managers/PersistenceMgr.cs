using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.Managers
{
    public class PersistenceMgr : MonoBehaviour
    {
        public static int MaxScore;
        public bool Loaded { get; private set; }


        #region Singleton
        public static PersistenceMgr Instance { get; private set; }

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

        private void Start()
        {
            Loaded = false;
        }

        public void Init()
        {
            MaxScore = 0;
            LoadGame();
        }
        
        public Game LoadGame()
        {
            Game game = new Game();
            // TODO Cargar de disco...
            Loaded = true;
            return game;
        } 

        public void SaveGame(Game game)
        {

        }

    } // Class

    [System.Serializable]
    public class Game
    {
        int maxScore;
    }
} // namespace