using UnityEngine;
using Unity.Collections; // Added for NativeArray

namespace GameCore.GridSystem
{
    /// <summary>
    /// Represents the game grid containing cells.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// Gets the number of rows in the grid.
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// Gets the number of columns in the grid.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// The 2D array storing the cells of the grid.
        /// </summary>
        private Cell[,] cells;

        /// <summary>
        /// Initializes a new instance of the Grid class with specified dimensions.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Grid(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
            {
                Debug.LogError($"Invalid grid dimensions: Rows ({rows}) and Columns ({columns}) must be positive.");
                Rows = 1;
                Columns = 1;
            }
            else
            {
                Rows = rows;
                Columns = columns;
            }
            cells = new Cell[Rows, Columns];
            // Ensure cells are initialized if needed, e.g., fill with default Cells
            // for (int r = 0; r < Rows; r++) {
            //     for (int c = 0; c < Columns; c++) {
            //         cells[r, c] = new Cell(' '); // Or some default character
            //     }
            // }
        }

        /// <summary>
        /// Gets or sets the cell at the specified row and column.
        /// Provides bounds checking.
        /// </summary>
        /// <param name="row">The row index (0-based).</param>
        /// <param name="column">The column index (0-based).</param>
        /// <returns>The Cell at the specified coordinates, or null if indices are out of bounds.</returns>
        public Cell this[int row, int column]
        {
            get
            {
                if (IsValidCoordinate(row, column))
                {
                    return cells[row, column];
                }
                Debug.LogError($"Attempted to access invalid grid coordinate: ({row}, {column})");
                return null;
            }
            set
            {
                if (IsValidCoordinate(row, column))
                {
                    // Ensure the cell object exists if setting for the first time
                    // Or handle null value assignment appropriately
                    cells[row, column] = value;
                }
                else
                {
                    Debug.LogError($"Attempted to set invalid grid coordinate: ({row}, {column})");
                }
            }
        }

        /// <summary>
        /// Checks if the given coordinates are within the bounds of the grid.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <returns>True if the coordinates are valid, false otherwise.</returns>
        public bool IsValidCoordinate(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        /// <summary>
        /// Converts the grid's characters into a flat NativeArray for use in Jobs.
        /// </summary>
        /// <param name="allocator">The allocator to use for the NativeArray.</param>
        /// <returns>A NativeArray containing grid characters (row-major order).</returns>
        public NativeArray<char> ToNativeCharArray(Allocator allocator)
        {
            var flatArray = new NativeArray<char>(Rows * Columns, allocator, NativeArrayOptions.UninitializedMemory);
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    // Handle null cells if they can occur
                    flatArray[r * Columns + c] = (cells[r, c] != null) ? cells[r, c].Character : '\0'; // Use null char for empty/null cells
                }
            }
            return flatArray;
        }

    }
}