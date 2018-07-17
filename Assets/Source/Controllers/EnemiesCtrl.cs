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
            if (enemies == 0)
                return;

            List<GameObject> goEnemies = GetRandomEnemies(enemies);

            for (int i = 0; i < enemies; i ++)
            {
                Enemy enemy = new Enemy(Instantiate(goEnemies[i]).transform);
                enemy.cellPos = GetRandomCell();
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

        private Vector2 GetRandomCell()
        {
            Vector2 cellPos = Vector2.zero;
            bool dif = false;
            while (!dif)
            {
                cellPos = new Vector2(Random.Range(1, 10) * 3,
                    Random.Range(1, 7) * 3);
                dif = true;
                if (enemiesList.Count != 0)
                    for (int i = 0; i < enemiesList.Count; i++)
                    {
                        if (cellPos.x - 3 < enemiesList[i].cellPos.x &&
                            enemiesList[i].cellPos.x < cellPos.x + 3)
                            
                                dif = false;
                    }
            }
            return cellPos;
        }

        public bool TestEnemyIn(Vector2 cell)
        {
            if (enemiesList.Count == 0)
                return false;
            else
            {
                for(int i = 0; i < enemiesList.Count; i++)
                {
                    if (enemiesList[i].cellPos.y == cell.y)
                        if(cell.x - 2 < enemiesList[i].cellPos.x && enemiesList[i].cellPos.x < cell.x + 2)
                        return true;
                }
                return false;
            }
        }

        public void DestroyEnemies()
        {
            if (enemiesList.Count > 0)
            {
                int count = enemiesList.Count;
                for (int i = 0; i < count; i++)
                {
                    Transform transf = enemiesList[0].primarySprite;
                    enemiesList.RemoveAt(0);
                    Destroy(transf.gameObject);
                }
            }
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