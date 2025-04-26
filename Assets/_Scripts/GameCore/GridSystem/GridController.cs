using UnityEngine;

namespace GameCore.GridSystem
{
    public class GridController : MonoBehaviour
    {
        /// <summary>
        /// Defines the dimensions (columns x rows) of the grid.
        /// Note: Vector2 components are float, cast to int when using.
        /// </summary>
        public static Vector2 GridSize = new Vector2(5, 5); 

        /// <summary>
        /// Constant string containing all characters of the Turkish alphabet.
        /// </summary>
        public const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ"; 

        /// <summary>
        /// Holds the reference to the current game grid instance.
        /// </summary>
        private Grid currentGrid;

        /// <summary>
        /// Public getter for the current grid instance.
        /// </summary>
        public Grid CurrentGrid => currentGrid; 

        /// <summary>
        /// Creates and initializes a new game grid based on GridSize.
        /// Each cell is populated with a random character from the Turkish alphabet.
        /// </summary>
        public void CreateGrid()
        {
            int rows = Mathf.RoundToInt(GridSize.y);
            int cols = Mathf.RoundToInt(GridSize.x);

            if (rows <= 0 || cols <= 0)
            {
                Debug.LogError("Invalid GridSize defined in GridController. Rows and Columns must be positive.");
                return;
            }

            currentGrid = new Grid(rows, cols);

            for (int r = 0; r < currentGrid.Rows; r++)
            {
                for (int c = 0; c < currentGrid.Columns; c++)
                {
                    
                    int randomIndex = Random.Range(0, TurkishAlphabet.Length);
                    char randomChar = TurkishAlphabet[randomIndex];

                    
                    Cell newCell = new Cell(randomChar);

                    
                    currentGrid[r, c] = newCell;
                }
            }

            Debug.Log($"Grid created with size {currentGrid.Columns}x{currentGrid.Rows}");
            
        }

        
        void Start()
        {
            CreateGrid();
        }
    }
}
