namespace GameCore.GridSystem
{
    /// <summary>
    /// Represents a single cell within the game grid, holding a character.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The character stored in this cell.
        /// </summary>
        public char Character { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Cell class with a specific character.
        /// </summary>
        /// <param name="character">The character for this cell.</param>
        public Cell(char character)
        {
            Character = character;
        }

    }
}