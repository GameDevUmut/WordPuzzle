using NUnit.Framework;
using UnityEngine;
using GameCore.GridSystem;

namespace GameCore.Tests.GridSystemTests
{
    [TestFixture]
    public class GridControllerTest
    {
        [Test]
        public void CreateGrid_ShouldInitializeGridWithRandomCharacters()
        {
            // Arrange
            var gameObject = new GameObject();
            var gridController = gameObject.AddComponent<GridController>();
            int expectedRows = Mathf.RoundToInt(GridController.GridSize.y);
            int expectedCols = Mathf.RoundToInt(GridController.GridSize.x);

            // Act
            gridController.CreateGrid();

            // Assert
            Assert.IsNotNull(gridController.CurrentGrid, "Grid should not be null after creation.");
            Assert.AreEqual(expectedRows, gridController.CurrentGrid.Rows, "Grid row count should match expected size.");
            Assert.AreEqual(expectedCols, gridController.CurrentGrid.Columns, "Grid column count should match expected size.");

            bool allCharsValid = true;
            bool differentCharsFound = false;
            char firstChar = '\0'; // Initialize with null character

            for (int r = 0; r < gridController.CurrentGrid.Rows; r++)
            {
                for (int c = 0; c < gridController.CurrentGrid.Columns; c++)
                {
                    Cell cell = gridController.CurrentGrid[r, c];
                    Assert.IsNotNull(cell, $"Cell at ({r}, {c}) should not be null.");

                    char cellChar = cell.Character;
                    if (!GridController.TurkishAlphabet.Contains(cellChar))
                    {
                        allCharsValid = false;
                        Debug.LogError($"Invalid character '{cellChar}' found at ({r}, {c}).");
                        break;
                    }

                    if (r == 0 && c == 0)
                    {
                        firstChar = cellChar;
                    }
                    else if (cellChar != firstChar)
                    {
                        differentCharsFound = true;
                    }
                }
                if (!allCharsValid) break;
            }

            Assert.IsTrue(allCharsValid, "All cell characters should be from the Turkish alphabet.");
            Assert.IsTrue(differentCharsFound || (expectedRows * expectedCols <= 1), "Grid should ideally contain different characters for basic randomness check (unless grid size is 1x1 or smaller).");
            
            Object.DestroyImmediate(gameObject);
        }
    }
}
