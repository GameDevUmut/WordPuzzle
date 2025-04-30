using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface ITrieService
    {
        UniTask<List<string>> SearchPossibleWords();
        UniTask<bool> TestifyWord(string word);
    }
}
