using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using Grid = GameCore.GridSystem.Grid;

namespace GameCore.Trie
{
    public class Trie : IDisposable
    {
        private const int TurkishAlphabetSize = 26 + 6;
        private const int MaxPathLength = 64;

        private static readonly int[] dr = {-1, -1, -1, 0, 0, 1, 1, 1};
        private static readonly int[] dc = {-1, 0, 1, -1, 1, -1, 0, 1};
        private Allocator allocator;
        private bool isBuilt = false;

        private TrieNode root;
        private CultureInfo turkishCulture = new CultureInfo("tr-TR");

        public Trie(Allocator alloc = Allocator.Persistent)
        {
            allocator = alloc;
            root = new TrieNode();
        }

        #region IDisposable Members

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        #endregion

        private int MapCharToIndex(char c)
        {
            char lowerChar = char.ToLower(c, turkishCulture);
            if (lowerChar >= 'a' && lowerChar <= 'z') return lowerChar - 'a' + 1;
            switch (lowerChar)
            {
                case 'ç': return 27;
                case 'ğ': return 28;
                case 'ı': return 29;
                case 'ö': return 30;
                case 'ş': return 31;
                case 'ü': return 32;
                default: return 0;
            }
        }

        public void Build(List<string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                Debug.LogError("Dictionary cannot be null or empty.");
                return;
            }

            root = new TrieNode();
            isBuilt = false;
            int wordCount = 0;

            foreach (string word in dictionary)
            {
                if (string.IsNullOrEmpty(word)) continue;
                TrieNode currentNode = root;
                bool wordAdded = false;
                foreach (char c in word)
                {
                    int charIndex = MapCharToIndex(c);
                    if (charIndex == 0)
                    {
                        Debug.LogWarning($"Skipping invalid character '{c}' in word '{word}'.");
                        continue;
                    }

                    if (!currentNode.Children.TryGetValue(charIndex, out TrieNode nextNode))
                    {
                        nextNode = new TrieNode();
                        currentNode.Children.Add(charIndex, nextNode);
                    }

                    currentNode = nextNode;
                    wordAdded = true;
                }

                if (wordAdded)
                {
                    currentNode.IsEndOfWord = true;
                    wordCount++;
                }
            }

            isBuilt = true;
            Debug.Log($"Standard Trie built with {wordCount} valid words.");
        }

        public async UniTask<NativeList<FixedString64Bytes>> FindAllWordsInGrid(Grid grid)
        {
            if (!isBuilt)
            {
                Debug.LogError("Trie must be built.");
                return default;
            }

            if (grid == null || grid.Rows == 0 || grid.Columns == 0)
            {
                Debug.LogError("Invalid grid.");
                return default;
            }

            if (root.Children.Count == 0)
            {
                Debug.LogWarning("Trie is empty.");
                return new NativeList<FixedString64Bytes>(Allocator.Persistent);
            }

            var foundWords = await Task.Run(() =>
            {
                var wordsList = new NativeList<FixedString64Bytes>(grid.Rows * grid.Columns * 5, allocator);
                bool[,] visited = new bool[grid.Rows, grid.Columns];
                char[] currentPath = new char[MaxPathLength];

                for (int r = 0; r < grid.Rows; r++)
                {
                    for (int c = 0; c < grid.Columns; c++)
                    {
                        DepthFirstSearch(grid, r, c, root, 0, visited, currentPath, wordsList);
                    }
                }

                return wordsList;
            });

            return foundWords;
        }

        private void DepthFirstSearch(Grid grid, int row, int col, TrieNode currentNode, int pathLength,
            bool[,] visited, char[] currentPath, NativeList<FixedString64Bytes> foundWords)
        {
            char gridChar = grid[row, col]?.Character ?? '\0';
            if (gridChar == '\0') return;

            int charIndex = MapCharToIndex(gridChar);
            if (charIndex == 0) return;

            if (!currentNode.Children.TryGetValue(charIndex, out TrieNode nextNode))
            {
                return;
            }

            if (pathLength >= MaxPathLength) return;

            visited[row, col] = true;
            currentPath[pathLength] = gridChar;
            int currentPathLength = pathLength + 1;

            if (nextNode.IsEndOfWord)
            {
                var word = new FixedString64Bytes();
                for (int i = 0; i < currentPathLength; i++)
                {
                    word.Append(currentPath[i]);
                }

                if (word.Length > 0)
                {
                    foundWords.Add(word);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                int nextRow = row + dr[i];
                int nextCol = col + dc[i];

                if (nextRow >= 0 && nextRow < grid.Rows && nextCol >= 0 && nextCol < grid.Columns &&
                    !visited[nextRow, nextCol])
                {
                    DepthFirstSearch(grid,
                        nextRow,
                        nextCol,
                        nextNode,
                        currentPathLength,
                        visited,
                        currentPath,
                        foundWords);
                }
            }

            visited[row, col] = false;
        }

        public async UniTask<bool> FindSingleWord(string word)
        {
            if (!isBuilt)
            {
                Debug.LogError("Trie must be built before searching.");
                return false;
            }

            if (string.IsNullOrEmpty(word))
            {
                Debug.LogWarning("Search word cannot be null or empty.");
                return false;
            }

            if (root.Children.Count == 0)
            {
                Debug.LogWarning("Trie is empty.");
                return false;
            }

            return await Task.Run(() =>
            {
                TrieNode currentNode = root;
                foreach (char c in word)
                {
                    int charIndex = MapCharToIndex(c);
                    if (charIndex == 0) return false;

                    if (!currentNode.Children.TryGetValue(charIndex, out TrieNode nextNode))
                    {
                        return false;
                    }

                    currentNode = nextNode;
                }

                return currentNode.IsEndOfWord;
            });
        }

        private void DisposeInternal()
        {
            root = null;
            isBuilt = false;
        }

        ~Trie() { }

        #region Nested type: TrieNode

        private class TrieNode
        {
            public Dictionary<int, TrieNode> Children { get; } = new Dictionary<int, TrieNode>();
            public bool IsEndOfWord { get; set; } = false;
        }

        #endregion
    }
}
