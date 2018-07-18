using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack.Utilities
{
    public class GameScreenCoords : MonoBehaviour
    {
        
        /// Este borde controla el ancho de la zona de juego, 
        /// teniendo en cuenta que debe mantener la relación 
        /// 32/24 que corresponde a la cantidad de celdas que 
        /// describen la posición de los elementos.
        [Tooltip("Porcentaje (en decimales) del borde superior que dejaremos para el juego.")]
        [SerializeField] float upper = 0.1f;
        
        public static float subUnit { get; private set; }
        public static float Units { get; private set; }
        private static Vector3 origin = Vector3.zero;
        private static Vector3 tempV3 = Vector3.zero;

        public static bool UnitsReady { get; private set; }

        private void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {
            UnitsReady = false;

            DefineUnits();
        }
        
        private void DefineUnits()
        {
            Vector2 screenSize = GetComponent<RectTransform>().sizeDelta;

            Units = screenSize.y * (1 - upper * 2) / 24;
            Units *= transform.localScale.x;

            origin.x = -16 * Units;
            origin.y = -12 * Units;

            subUnit = Units / 3;

            UnitsReady = true;
        }

        public static Vector3 CellToWorld(int x, int y)
        {
            tempV3.x = origin.x + x * Units;
            tempV3.y = origin.y + y * Units;
            return tempV3;
        }

        public static Vector3 CellToWorld(Vector2 cell)
        {
            tempV3.x = origin.x + cell.x * Units;
            tempV3.y = origin.y + cell.y * Units;
            return tempV3;
        }

    } // CLass
} // namespace