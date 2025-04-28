
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.GridSystem;
using Interfaces;
using Unity.Collections;
using UnityEngine;
using VContainer;

namespace GameCore.Trie
{
    public class TrieManager : MonoBehaviour, ITrieService
    {
        private Trie _trie;
        private GridManager _gridManager;
        private IDictionaryService _dictionaryService;

        [Inject]
        private void Construct(GridManager gridManager, IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
            _gridManager = gridManager;
        }
        
        private void Awake()
        {
            CreateTrie();
        }
        
        private void OnDestroy()
        {
            _trie?.Dispose(); //we dispose trie objects on destroy
        }
        
        private async UniTask CreateTrie()
        {
            var dictionaryReadSuccess = await _dictionaryService.DictionaryLoadTask;
            if(!dictionaryReadSuccess)
            {
                Debug.LogError("Dictionary load failed. Not Creating Trie.");
                return;
            }
            
            _trie = new Trie();
            await _trie.Build(_dictionaryService.LoadedWords);
        }

        public async UniTask<List<string>> SearchPossibleWords()
        {
            NativeList<FixedString64Bytes> nativeList = default;
            List<string> returnList = new List<string>();

            try 
            {
                nativeList = await _trie.FindAllWordsInGrid(_gridManager.CurrentGrid);
                if (nativeList.IsCreated && nativeList.Length > 0)
                {
                    returnList.Capacity = nativeList.Length;
                    for (int i = 0; i < nativeList.Length; i++)
                    {
                        returnList.Add(nativeList[i].ToString());
                    }
                }
            }
            finally
            {
                
                if (nativeList.IsCreated)
                {
                    nativeList.Dispose();
                }
            }


            return returnList;
        }
        
        public async UniTask<bool> TestifyWord(string word)
        {
            return await _trie.FindSingleWord(word);
        }
    }
}
