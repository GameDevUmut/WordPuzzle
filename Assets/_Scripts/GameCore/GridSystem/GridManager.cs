using R3;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.GridSystem
{
    public class GridManager : MonoBehaviour, IGridService
    {
        /// <summary>
        /// Constant string containing all characters of the Turkish alphabet.
        /// </summary>
        public const string TurkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        /// <summary>
        /// Defines the dimensions (columns x rows) of the grid.
        /// Note: Vector2 components are float, cast to int when using.
        /// </summary>
        public static Vector2 GridSize = new Vector2(5, 5);

        #region Fields

        /// <summary>
        /// Holds the reference to the current game grid instance.
        /// </summary>
        private Grid currentGrid;
        private readonly Subject<Unit> _gridCreated = new();

        #endregion

        #region Properties

        /// <summary>
        /// Public getter for the current grid instance.
        /// </summary>
        public Grid CurrentGrid => currentGrid;

        #endregion

        #region Unity Methods

        void Awake()
        {
            // Initialize the subject before creating the grid
            // _gridCreated = new Subject<Unit>(); // It's better to initialize in the field declaration or constructor
            CreateGrid();
        }

        void OnDestroy()
        {
            _gridCreated.Dispose(); // Dispose the subject when the GameObject is destroyed
        }

        #endregion

        #region Public Methods

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
            // NotifyObservers(gameObject, ObserverEventType.GridCreated); // Removed old notification
            _gridCreated.OnNext(Unit.Default); // Trigger the R3 subject
        }
        
        #endregion

        #region IGridService Members

        public int GridRows => currentGrid.Rows;
        public int GridColumns => currentGrid.Columns;

        public char GetCellCharacter(int row, int column) => currentGrid[row, column].Character;

        public Observable<Unit> GridCreated => _gridCreated; // Implement the new interface property

        #endregion

        // public ISubject Subject => this; // Removed old property

    }
}
