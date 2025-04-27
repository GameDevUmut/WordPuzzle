using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Grid = GameCore.GridSystem.Grid;

namespace GameCore.Trie
{
    /// <summary>
    /// Represents a Double Array Trie for efficient prefix and word searching.
    /// Designed to be used with Unity's Job System and Burst Compiler.
    /// </summary>
    public class DoubleArrayTrie : IDisposable
    {
        private NativeArray<int> baseArray;
        private NativeArray<int> checkArray;
        private NativeArray<bool> terminalArray;
        private bool isBuilt = false;
        private Allocator allocator;
        
        private int MapCharToIndex(char c)
        {
            if (c >= 'a' && c <= 'z') return c - 'a' + 1;
            if (c >= 'A' && c <= 'Z') return c - 'A' + 1;
            return 0;
        }

        /// <summary>
        /// Initializes a new instance of the DoubleArrayTrie class.
        /// </summary>
        /// <param name="alloc">The allocator to use for NativeArrays.</param>
        public DoubleArrayTrie(Allocator alloc = Allocator.Persistent)
        {
            allocator = alloc;
            // Initialize with minimal size or leave uninitialized until Build
        }

        /// <summary>
        /// Builds the Double Array Trie from a given dictionary of words.
        /// </summary>
        /// <param name="dictionary">List of words to build the trie from.</param>
        public void Build(List<string> dictionary)
        {
            DisposeInternal(); // Dispose previous arrays if rebuilding
            
            int estimatedSize = 1024;
            baseArray = new NativeArray<int>(estimatedSize, allocator);
            checkArray = new NativeArray<int>(estimatedSize, allocator);
            terminalArray = new NativeArray<bool>(estimatedSize, allocator);
            
            Debug.LogWarning("DoubleArrayTrie.Build() - Actual construction logic is not implemented.");


            isBuilt = true;
        }

        /// <summary>
        /// Finds all words from the pre-built dictionary within the given grid.
        /// Uses Unity's Job System and Burst Compiler for parallel searching.
        /// </summary>
        /// <param name="grid">The grid to search in.</param>
        /// <returns>A JobHandle for the scheduled search jobs and a NativeList containing found words (caller must Dispose).</returns>
        public (JobHandle, NativeList<FixedString64Bytes>) FindAllWordsInGrid(Grid grid)
        {
            if (!isBuilt)
            {
                Debug.LogError("Trie must be built before searching.");
                return (default, default);
            }
            if (grid == null || grid.Rows == 0 || grid.Columns == 0)
            {
                 Debug.LogError("Invalid grid provided.");
                return (default, default);
            }
            
            NativeArray<char> gridChars = grid.ToNativeCharArray(Allocator.TempJob);
            
            NativeList<FixedString64Bytes> foundWords = new NativeList<FixedString64Bytes>(grid.Rows * grid.Columns * 5, Allocator.TempJob);

            // Create and Schedule Jobs
            var searchJob = new GridSearchJob
            {
                GridChars = gridChars,
                Rows = grid.Rows,
                Columns = grid.Columns,
                BaseArray = baseArray,
                CheckArray = checkArray,
                TerminalArray = terminalArray,
                FoundWords = foundWords.AsParallelWriter() // Use ParallelWriter for thread safety
            };
            
            JobHandle handle = searchJob.Schedule(grid.Rows * grid.Columns, 32); // Adjust batch size (32) as needed
            return (handle, foundWords);
        }


        /// <summary>
        /// Disposes the internal NativeArrays.
        /// </summary>
        private void DisposeInternal()
        {
             if (baseArray.IsCreated) baseArray.Dispose();
             if (checkArray.IsCreated) checkArray.Dispose();
             if (terminalArray.IsCreated) terminalArray.Dispose();
             isBuilt = false;
        }

        /// <summary>
        /// Public Dispose method.
        /// </summary>
        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        ~DoubleArrayTrie()
        {
            // Ensure disposal
             if (baseArray.IsCreated || checkArray.IsCreated || terminalArray.IsCreated)
             {
                 Debug.LogWarning("DoubleArrayTrie was not disposed. Remember to call Dispose().");
             }
        }
    }
}

