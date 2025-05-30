﻿
using System.Collections.Generic;
using System.Linq;
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
        private static Trie _trie;
        private GridManager _gridManager;
        private IDictionaryService _dictionaryService;
        private IGameService _gameService;

        [Inject]
        private void Construct(GridManager gridManager, IDictionaryService dictionaryService, IGameService gameService)
        {
            _gameService = gameService;
            _dictionaryService = dictionaryService;
            _gridManager = gridManager;
        }
        
        private void Awake()
        {
            if (_trie != null && _trie.IsBuilt) return;
                
            CreateTrie().Forget();
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

            return returnList.Distinct().ToList();
        }
        
        public async UniTask<bool> TestifyWord(string word)
        {
            var result = await _trie.FindSingleWord(word);
            if(result)
                _gameService.AddFoundWord(word);
            return result;
        }
    }
}
