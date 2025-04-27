using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace GameCore.Trie
{
    [BurstCompile(CompileSynchronously = true)] //TODO: Adjust
    public struct GridSearchJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<char> GridChars;
        [ReadOnly] public int Rows;
        [ReadOnly] public int Columns;

        [ReadOnly] public NativeArray<int> BaseArray;
        [ReadOnly] public NativeArray<int> CheckArray;
        [ReadOnly] public NativeArray<bool> TerminalArray;

        public NativeList<FixedString64Bytes>.ParallelWriter FoundWords;

        private static readonly int[] dr = {-1, -1, -1, 0, 0, 1, 1, 1};
        private static readonly int[] dc = {-1, 0, 1, -1, 1, -1, 0, 1};

        private const int MaxPathLength = 64;


        // TODO: Implement character mapping consistent with DoubleArrayTrie
        private int MapCharToIndex(char c)
        {
            if (c >= 'a' && c <= 'z') return c - 'a' + 1;
            if (c >= 'A' && c <= 'Z') return c - 'A' + 1;
            return 0;
        }

        public void Execute(int index)
        {
            int startRow = index / Columns;
            int startCol = index % Columns;

            var visited = new NativeArray<bool>(Rows * Columns, Allocator.Temp, NativeArrayOptions.ClearMemory);
            var currentPath =
                new NativeArray<char>(MaxPathLength, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

            // Start DFS
            DepthFirstSearch(startRow, startCol, 0, 0, ref visited, ref currentPath);

            // Dispose temporary allocations
            visited.Dispose();
            currentPath.Dispose();
        }

        private void DepthFirstSearch(int row, int col, int trieNodeIndex, int pathLength,
            ref NativeArray<bool> visited, ref NativeArray<char> currentPath)
        {
            char currentChar = GridChars[row * Columns + col];
            int charIndex = MapCharToIndex(currentChar);
            if (charIndex == 0) return;

            int nextTrieNodeIndex = BaseArray[trieNodeIndex] + charIndex;

            if (nextTrieNodeIndex < 0 || nextTrieNodeIndex >= CheckArray.Length ||
                CheckArray[nextTrieNodeIndex] != trieNodeIndex)
            {
                return;
            }

            if (pathLength >= MaxPathLength) return;
            currentPath[pathLength] = currentChar;
            visited[row * Columns + col] = true;
            int currentPathLength = pathLength + 1;


            if (TerminalArray[nextTrieNodeIndex])
            {
                var word = new FixedString64Bytes();
                for (int i = 0; i < currentPathLength; ++i)
                {
                    word.Append(currentPath[i]);
                }

                if (word.Length > 0)
                {
                    FoundWords.AddNoResize(word);
                }
            }
            
            for (int i = 0; i < 8; i++)
            {
                int nextRow = row + dr[i];
                int nextCol = col + dc[i];
                
                if (nextRow >= 0 && nextRow < Rows && nextCol >= 0 && nextCol < Columns &&
                    !visited[nextRow * Columns + nextCol])
                {
                    DepthFirstSearch(nextRow,
                        nextCol,
                        nextTrieNodeIndex,
                        currentPathLength,
                        ref visited,
                        ref currentPath);
                }
            }
            
            visited[row * Columns + col] = false;
        }
    }
}
