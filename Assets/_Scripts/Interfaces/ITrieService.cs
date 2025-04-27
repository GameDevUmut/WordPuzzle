using System.Collections.Generic;
using Unity.Collections;

namespace Interfaces
{
    public interface ITrieService
    {
        List<string> Search();
        bool TestifyWord(string word);
    }
}
