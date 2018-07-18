using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Utilities;

namespace JumpingJack.Controllers
{
    public class EnemiesCtrl : MonoBehaviour
    {
        public GameObject[] enemyPrefabsList;
        List<Enemy> enemiesList = new List<Enemy>();
        public static bool ready = false;

        public Color[] enemyColors;
        private enum EnemyColors { Blue,Green,Magenta,Yellow,Black,Red}
        private Dictionary<int, EnemyColors> enemyColorsDic = new Dictionary<int, EnemyColors>();

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

            enemyColorsDic.Add(0, EnemyColors.Blue);
            enemyColorsDic.Add(6, EnemyColors.Blue);
            enemyColorsDic.Add(12, EnemyColors.Blue);
            enemyColorsDic.Add(18, EnemyColors.Blue);
            enemyColorsDic.Add(1, EnemyColors.Green);
            enemyColorsDic.Add(7, EnemyColors.Green);
            enemyColorsDic.Add(13, EnemyColors.Green);
            enemyColorsDic.Add(19, EnemyColors.Green);
            enemyColorsDic.Add(2, EnemyColors.Magenta);
            enemyColorsDic.Add(8, EnemyColors.Magenta);
            enemyColorsDic.Add(14, EnemyColors.Magenta);
            enemyColorsDic.Add(20, EnemyColors.Magenta);
            enemyColorsDic.Add(3, EnemyColors.Yellow);
            enemyColorsDic.Add(9, EnemyColors.Yellow);
            enemyColorsDic.Add(15, EnemyColors.Yellow);
            enemyColorsDic.Add(21, EnemyColors.Yellow);
            enemyColorsDic.Add(4, EnemyColors.Black);
            enemyColorsDic.Add(10, EnemyColors.Black);
            enemyColorsDic.Add(16, EnemyColors.Black);
            enemyColorsDic.Add(5, EnemyColors.Red);
            enemyColorsDic.Add(11, EnemyColors.Red);
            enemyColorsDic.Add(17, EnemyColors.Red);


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


        public void InstanceEnemies(int _enemies)
        {
            StartCoroutine((InstanceWithDelay(_enemies)));
        }
        public void printSomething()
        {
            Debug.Log("Algo1");
            Debug.Log("Algo2");

        }

        List<GameObject> goList = new List<GameObject>();
        private IEnumerator InstanceWithDelay(int countEnemies)
        {
            GerRandomIndex(ref prefabsRandomIndex, countEnemies);

            for (int i = 0; i < countEnemies; i++)
            {
                yield return new WaitForFixedUpdate();
                goList.Add(Instantiate(enemyPrefabsList[prefabsRandomIndex[i]
                    ]));
                
            }
            for (int i = 0; i< countEnemies; i++)
            {
                Enemy enemy = new Enemy();
                enemy.primarySprite = goList[i].GetComponent<Transform>();

                enemy.primarySprite.GetComponentInChildren<SpriteRenderer>().color = GetColor(i);

                enemy.cellPos = GetRandomCell();

                enemy.SetScale(new Vector3(GameScreenCoords.Units,
                GameScreenCoords.Units, 1));

                enemiesList.Add(enemy);
            }
        }
        
        public void Init(int enemies)
        {
            Debug.Log("0");
            if (enemies == 0)
                return;

            GerRandomIndex(ref prefabsRandomIndex, enemies);
            for (int i = 0; i < enemies; i ++)
            {
                Enemy enemy = new Enemy();
                    enemy.primarySprite = Instantiate(enemyPrefabsList[prefabsRandomIndex[i]]).GetComponent<Transform>();

                enemy.primarySprite.GetComponentInChildren<SpriteRenderer>().color =
                    GetColor(i);

                enemy.cellPos = GetRandomCell();
                enemiesList.Add(enemy);
            }
        }
        
        private Color GetColor(int index)
        {
            switch (enemyColorsDic[index])
            {
                case EnemyColors.Blue:
                    return enemyColors[0];
                case EnemyColors.Green:
                    return enemyColors[1];
                case EnemyColors.Magenta:
                    return enemyColors[2];
                case EnemyColors.Yellow:
                    return enemyColors[3];
                case EnemyColors.Black:
                    return enemyColors[4];
                case EnemyColors.Red:
                    return enemyColors[5];
                default:
                    return Color.white;
            }
        }

        private List<GameObject> GetRandomEnemies(int count)
        {
            List<GameObject> outEnemies = new List<GameObject>();

            List<GameObject> tempEnemies = new List<GameObject>();
            
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (j == enemyPrefabsList.Length)
                    j = 0;

                tempEnemies.Add(enemyPrefabsList[j]);
                j++;
            }

            for(int i = 0; i< count; i++)
            {
                int index = Random.Range(0, tempEnemies.Count);
                outEnemies.Add(tempEnemies[index]);
                tempEnemies.RemoveAt(index);
            }
            
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
            while (!dif)
            {
                cellPos = new Vector2(Random.Range(0,29),
                    Random.Range(1, 8)*3);
                dif = true;
                if (enemiesList.Count != 0)
                {
                    for (int i = 0; i < enemiesList.Count; i++)
                    {
                        if (cellPos.y != enemiesList[i].cellPos.y)
                            continue;
                        if (cellPos.x - 4 >= enemiesList[i].cellPos.x ||
                            (cellPos.x + 4 <= enemiesList[i].cellPos.x))
                            continue;
                        else
                        {
                            dif = false;
                            break;
                        }
                    }
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

        public Enemy() { }

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