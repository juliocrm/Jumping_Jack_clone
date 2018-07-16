using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Utilities;

namespace JumpingJack.Controllers
{
    public class EnemiesCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemiesPrefab;
        List<Enemy> enemiesList = new List<Enemy>();
        
        #region Singleton
        public static EnemiesCtrl Instance { get; private set; }

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

        public void Tic(int frame)
        {
            for (int i = 0; i < enemiesList.Count; i++)
                enemiesList[i].Tick(frame);
        }

        public void Init(int enemies)
        {
            List<GameObject> goEnemies = GetRandomEnemies(enemies);

            for (int i = 0; i < enemies; i ++)
            {
                Enemy enemy = new Enemy(Instantiate(goEnemies[i]).transform);
                enemiesList.Add(enemy);
                enemiesList[i].SetScale(new Vector3(GameScreenCoords.Units,
                                                    GameScreenCoords.Units,1));
            }
        }

        private List<GameObject> GetRandomEnemies(int count)
        {
            List<GameObject> tempEnemies = new List<GameObject>();
            tempEnemies.AddRange(enemiesPrefab);
            List<GameObject> outEnemies = new List<GameObject>();
            for(int i = 0; i< count; i++)
            {
                int index = Random.Range(0, tempEnemies.Count);
                outEnemies.Add(tempEnemies[index]);
                tempEnemies.RemoveAt(index);
            }
            return outEnemies;
        }

        public void ResetController()
        {
            
        }

    } // EnemiesCtrk

    public class Enemy
    {
        public Transform primarySprite;
        public Transform secondarySprite;
        public Vector2 cellPos = new Vector2();

        public Enemy(Transform _transform)
        {
            primarySprite = _transform;
        }

        public void Tick(int frame)
        {
            primarySprite.position = GameScreenCoords.CellToWorld(cellPos);

            if (frame == 4)
            {
                if (cellPos.x == 0)
                {
                    cellPos.x = 30;
                    if (cellPos.y + 3 > 23)
                    {
                        if (cellPos.y + 3 == 24) {
                            cellPos.y = 27;
                            cellPos.x = 21;
                        }
                        else if (cellPos.y == 27)
                            cellPos.y = 3;
                    }
                    else
                        cellPos.y += 3;
                }
                else
                    cellPos.x--;
            }
        }

        public void SetScale(Vector3 scale)
        {
            primarySprite.localScale = scale;
        }
    } // Enemy

} // namespace