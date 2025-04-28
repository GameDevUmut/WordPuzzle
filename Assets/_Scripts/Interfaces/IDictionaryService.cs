using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IDictionaryService
    {
        public UniTask DictionaryLoadTask { get; }

        public List<string> LoadedWords { get; }
    }
}
