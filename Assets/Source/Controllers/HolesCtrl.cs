using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JumpingJack.Utilities;

namespace JumpingJack.Controllers
{
    public class HolesCtrl : MonoBehaviour
    {
        [SerializeField] private GameObject holeMaskPrefab;
        //[SerializeField] GameObject[] rightHoles;
        //[SerializeField] GameObject[] leftHoles;
        
        Hole[] holesRandomIndex = new Hole[8];
        int[] randomLines = new int[8];
        List<Hole> holesList = new List<Hole>();

        private int holesActivated = 2;

        // Unidades resultado de dividir el lado 
        // de las celdas entre 4. Cada agujero se
        // mueve un cuarto de celda cada frame.
        private float subUnits;

        #region Singleton
        public static HolesCtrl Instance { get; private set; }

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
        IEnumerator Start()
        {
            yield return new WaitWhile(() =>
            GameScreenCoords.UnitsReady == false);

            subUnits = GameScreenCoords.Units / 4;
        }

        public void Init()
        {
        }

        public void Tic(int frame)
        {
            for (int i = 0; i < holesList.Count; i++)
            {
                holesList[i].Tic(frame);
            }
        }

        public void AddHoles(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (holesList.Count < 8)
                    holesList.Add(GetRandomHole());
            }
        }

        public void AddHoleIn(Vector2 cell, int direction)
        {
            Hole hole = GetRandomHole();
            hole.cellPos = cell;
            hole.moveDirection = direction;
            holesList.Add(hole);
        }

        public void DestroyHoles()
        {
            if (holesList.Count > 0) {
                int count = holesList.Count;
                for (int i = 0; i < count; i++)
                {
                    Transform transf = holesList[0].primarySprite;
                    holesList.RemoveAt(0);
                    Destroy(transf.gameObject);
                }
            }
        }

        private Hole GetRandomHole()
        {
            Hole hole = new Hole();

            Vector2 cellPos = Vector2.zero;
            int direction = 0;

            bool dif = false;
            while(!dif)
            {
                cellPos = new Vector2(Random.Range(0, 10) * 3,
                    Random.Range(1, 9)*3);
                direction = GetRandomDirection();
                dif = true;
                if(holesList.Count != 0)
                for (int i = 0; i < holesList.Count; i++)
                {
                    if (cellPos.x - 4 < holesList[i].cellPos.x &&
                        holesList[i].cellPos.x < cellPos.x + 4)

                        if (holesList[i].moveDirection == direction)
                            dif = false;
                }
            }
            hole.cellPos = cellPos;
            hole.moveDirection = direction;
            hole.primarySprite = Instantiate(holeMaskPrefab).transform;
            hole.SetScale(new Vector3(GameScreenCoords.Units,
                                        GameScreenCoords.Units, 1));
            
            return hole;
        }
        
        private int GetRandomDirection()
        {
            int dir;
            do
            {
                dir = Random.Range(-1, 2);
            } while (dir == 0);
            return dir;                    
        }

        #region Test holes positions
        public bool ExistHoleUp(Vector2 avatarCell)
        {
            avatarCell.y += 3;
            for(int i = 0; i < holesList.Count; i++)
            {
                if (holesList[i].cellPos.x <= avatarCell.x &&
                    holesList[i].cellPos.x <= avatarCell.x + 2)
                {
                    if (holesList[i].cellPos.y == avatarCell.y)
                        return true;
                }
            }
            return false;
        }

        public bool ExistHoleDown(Vector2 avatarCell)
        {
            for (int i = 0; i < holesList.Count; i++)
            {
                if (holesList[i].cellPos.y == avatarCell.y)
                {
                    if (holesList[i].cellPos.x == avatarCell.x ||
                    holesList[i].cellPos.x + 1 == avatarCell.x)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

    } // Class

    public class Hole
    {
        public int moveDirection = 0;
        
        public Transform primarySprite;
        public GameObject secondarySprite;

        public Vector2 cellPos = new Vector2();

        public Hole(Vector2 cellPosition, int direction)
        {
            moveDirection = direction;
            cellPos = cellPosition;

        }
        public Hole(){}

        public void UpdatePos(int cell, int line)
        {
            primarySprite.transform.position = GameScreenCoords.CellToWorld(cell, line);
        }

        public void Tic(int frame)
        {
            if (frame == 4)
            {
                primarySprite.position = GameScreenCoords.CellToWorld(cellPos);

                if (moveDirection == 1)
                {
                    if (cellPos.x == 30)
                    {
                        cellPos.x = 0;
                        if (cellPos.y - 3 < 3)
                            cellPos.y = 24;
                        else
                            cellPos.y -= 3;
                    }
                    else
                        cellPos.x++;
                }
                else
                {
                    if (cellPos.x == 0)
                    {
                        cellPos.x = 30;
                        if (cellPos.y + 3 > 24)
                            cellPos.y = 3;
                        else
                            cellPos.y += 3;
                    }
                    else
                        cellPos.x--;
                }
            }

        }

        public void SetScale(Vector3 scale){
            primarySprite.localScale = scale;
        }

    } // Hole
} // namespace