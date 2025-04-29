using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IDictionaryService
    {
        UniTask<bool> DictionaryLoadTask { get; }

        List<string> LoadedWords { get; }
    }
}
