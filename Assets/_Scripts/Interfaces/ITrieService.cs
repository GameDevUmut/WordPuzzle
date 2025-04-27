using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Collections;

namespace Interfaces
{
    public interface ITrieService
    {
        UniTask<List<string>> SearchPossibleWords();
        UniTask<bool> TestifyWord(string word);
    }
}
