using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        
        public static void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/Gamedt.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/Gamedt.dat", FileMode.Open);

                Game game = bf.Deserialize(file) as Game;
                file.Close();

                MaxScore = game.maxScore;
            }
            Instance.Loaded = true;
        } 

        public static void SaveGame(int maxScore)
        {
            MaxScore = maxScore;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/Gamedt.dat");

            Game game = new Game(maxScore);
            bf.Serialize(file, game);
            file.Close();
        }

    } // Class

    [System.Serializable]
    public class Game
    {
        public int maxScore;
        public Game(int _maxScore)
        {
            maxScore = _maxScore;
        }
    }
} // namespace