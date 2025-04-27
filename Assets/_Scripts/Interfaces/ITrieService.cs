using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Collections;

namespace Interfaces
{
    public interface ITrieService
    {
        UniTask<List<string>> Search();
        bool TestifyWord(string word);
    }
}
