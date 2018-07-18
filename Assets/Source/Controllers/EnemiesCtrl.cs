using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Utilities;

namespace JumpingJack.Controllers
{
    public class EnemiesCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabsList;
        List<Enemy> enemiesList = new List<Enemy>();
        public static bool ready = false;
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
            ready = true;
        }
        

        public void Tic(int frame)
        {
            if (enemiesList.Count > 0)
            {
                for (int i = 0; i < enemiesList.Count; i++)
                    enemiesList[i].Tick(frame);
            }
        }

        List<int> prefabsRandomIndex = new List<int>();

        public void Init(int enemies)
        {
            Debug.Log("0");
            if (enemies == 0)
                return;

            GerRandomIndex(ref prefabsRandomIndex, enemies);
            //prefabsRandomIndex.Add(0);
            //prefabsRandomIndex.Add(1);
            //prefabsRandomIndex.Add(2);
            //prefabsRandomIndex.Add(3);
            //prefabsRandomIndex.Add(4);
            //prefabsRandomIndex.Add(5);
            //prefabsRandomIndex.Add(6);
            //prefabsRandomIndex.Add(7);
            //prefabsRandomIndex.Add(8);
            //prefabsRandomIndex.Add(9);
            //prefabsRandomIndex.Add(1);



            Debug.Log("0_0");
            for (int i = 0; i < enemies; i ++)
            {
                Debug.Log("Instanciando enemies");
                Enemy enemy = new Enemy(Instantiate(enemyPrefabsList[prefabsRandomIndex[i]]).transform);

                enemy.cellPos = GetRandomCell();
                enemiesList.Add(enemy);
                enemiesList[i].SetScale(new Vector3(GameScreenCoords.Units,
                                                    GameScreenCoords.Units,1));
            }
        }

        private List<GameObject> GetRandomEnemies(int count)
        {
            List<GameObject> outEnemies = new List<GameObject>();

            List<GameObject> tempEnemies = new List<GameObject>();
            Debug.Log("1");
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (j == enemyPrefabsList.Length)
                    j = 0;

                tempEnemies.Add(enemyPrefabsList[j]);
                j++;
            }
            Debug.Log("2: " + tempEnemies.Count);
            for(int i = 0; i< count; i++)
            {
                int index = Random.Range(0, tempEnemies.Count);
                outEnemies.Add(tempEnemies[index]);
                tempEnemies.RemoveAt(index);
            }
            /*
            Debug.Log("enemies ready");
            */
            return outEnemies;
        }

        private void GerRandomIndex(ref List<int> indexList, int count)
        {
            List<int> tempList = new List<int>();
            for(int i = 0; i < 10; i++)
            {
                tempList.Add(i);
            }
            if (count <= 10)
            {
                for (int i = 0; i < count; i++)
                {
                    int ind = Random.Range(0, tempList.Count);
                    indexList.Add(tempList[ind]);
                    tempList.RemoveAt(ind);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int ind = Random.Range(0, tempList.Count);
                    indexList.Add(tempList[ind]);
                    tempList.RemoveAt(ind);
                }
            }
            if (count > indexList.Count)
                GerRandomIndex(ref indexList, count - indexList.Count);
        }

        private Vector2 GetRandomCell()
        {
            Vector2 cellPos = Vector2.zero;
            bool dif = false;
            Debug.Log("3");
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
            Debug.Log("Random Finished");
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