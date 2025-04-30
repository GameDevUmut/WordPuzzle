using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.GridSystem;
using GameCore.Trie;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using Grid = GameCore.GridSystem.Grid;

namespace GameCore.Tests.TrieTests
{
    [TestFixture]
    public class TrieTestSuite
    {
        [Test]
        public async Task CreateGridAndTrie_TrySearch()
        {
            // Arrange
            List<string> words = new List<string>()
            {
                "Merhaba",
                "Alo",
                "Ana",
                "Bir",
                "Cep",
                "Do",
                "Ev",
                "Can",
                "Ben",
                "Arı",
                "Su",
                "Ay",
                "El",
                "Bal",
                "Gül",
                "Kuş",
                "Yol",
                "Deniz",
                "Taş",
                "Kum",
                "Dağ",
                "Kar",
                "Ses",
                "Dur",
                "Gel",
                "Git",
                "Kal",
                "Yaz",
                "Kış",
                "Bahçe",
                "Evren",
                "Yıldız",
                "Ayak",
                "Kedi",
                "Köpek",
                "Balık",
                "Çiçek",
                "Ağaç",
                "Top",
                "Kapı",
                "Cam",
                "Buz",
                "Işık",
                "Rüzgar",
                "Yağmur",
                "Bulut",
                "Göl",
                "Nehir",
                "Orman",
                "Köy",
                "Şehir"
            };

            GridManager.GridSize = new Vector2(10, 10);
            var gameObject = new GameObject();
            var gridController = gameObject.AddComponent<GridManager>();
            int expectedRows = Mathf.RoundToInt(GridManager.GridSize.y);
            int expectedCols = Mathf.RoundToInt(GridManager.GridSize.x);

            // Act
            gridController.CreateGrid();
            
            Grid grid = gridController.CurrentGrid;
            
            // --- Log Grid Characters ---
            Debug.Log("--- Grid Content (10x10) ---");
            StringBuilder gridLog = new StringBuilder();
            for (int row = 0; row < grid.Rows; row++)
            {
                for (int col = 0; col < grid.Columns; col++)
                {
                    gridLog.Append(grid[row, col]?.Character ?? '?');
                }
                Debug.Log(gridLog.ToString());
                gridLog.Clear();
            }
            Debug.Log("--- End Grid Content ---");
            // --- End Log Grid Characters ---

            //Assert
            Assert.AreEqual(grid.Rows, expectedRows, "Grid row count should match expected size.");
            Assert.AreEqual(grid.Columns, expectedCols, "Grid column count should match expected size.");
            Assert.IsNotNull(grid[1,1], "Grid should exist.");
            
            try
            {
                using (Trie.Trie trie = new Trie.Trie())
                {
                    await trie.Build(words);
                    
                    Debug.Log($"Trie built with {words.Count} words.");

                    var nativeList = Task.Run(async () => await trie.FindAllWordsInGrid(grid)).GetAwaiter().GetResult();
                    Debug.Log($"Found words count: {nativeList.Length}");
                    if (nativeList.Length > 0)
                    {
                        Debug.Log("--- Found Words ---");
                        foreach (var word in nativeList)
                        {
                            Debug.Log(word.ToString());
                        }
                        Debug.Log("--- End Found Words ---");
                    }
                    
                    if (nativeList.IsCreated)
                    {
                        nativeList.Dispose();
                    }

                }
            } catch (System.Exception e)
            {
                Debug.LogError($"Error during Trie operations: {e.Message}");
            }
   
            
            Object.DestroyImmediate(gameObject);
        }
    }
}
