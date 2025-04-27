using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.GridSystem;
using Interfaces;
using Unity.Collections;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace GameCore.Trie
{
    public class TrieManager : MonoBehaviour, ITrieService
    {
        Trie _trie = new Trie();
        
        private List<string> _dictionary = new List<string>()
        {
            "Merhaba",
            "Alo",
            "Ana",
            "Bir",
            "Cep",
            "Do",
            "Ev",
            "Can",
            "Ben",
            "Arı",
            "Su",
            "Ay",
            "El",
            "Bal",
            "Gül",
            "Kuş",
            "Yol",
            "Deniz",
            "Taş",
            "Kum",
            "Dağ",
            "Kar",
            "Ses",
            "Dur",
            "Gel",
            "Git",
            "Kal",
            "Yaz",
            "Kış",
            "Bahçe",
            "Evren",
            "Yıldız",
            "Ayak",
            "Kedi",
            "Köpek",
            "Balık",
            "Çiçek",
            "Ağaç",
            "Top",
            "Kapı",
            "Cam",
            "Buz",
            "Işık",
            "Rüzgar",
            "Yağmur",
            "Bulut",
            "Göl",
            "Nehir",
            "Orman",
            "Köy",
            "Şehir"
        };
        private GridManager _gridManager;

        [Inject]
        private void Construct(GridManager gridManager)
        {
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
        
        private void CreateTrie()
        {
            _trie = new Trie();
            _trie.Build(_dictionary);
        }

        public List<string> Search()
        {
            NativeList<FixedString64Bytes> nativeList = default;
            List<string> returnList = new List<string>();

            try // Ensure nativeList is disposed even if errors occur
            {
                var result = _trie.FindAllWordsInGrid(_gridManager.CurrentGrid);

                // NOTE: Since StandardTrie.FindAllWordsInGrid is now synchronous,
                // result.Item1 (JobHandle) is default and Complete() does nothing,
                // but it's harmless to leave it for API compatibility if switching back.
                // result.Item1.Complete(); // No longer strictly necessary for the sync StandardTrie

                nativeList = result.Item2;

                // Manual conversion from NativeList<FixedString...> to List<string>
                if (nativeList.IsCreated && nativeList.Length > 0)
                {
                    returnList.Capacity = nativeList.Length; // Optimize list capacity
                    for (int i = 0; i < nativeList.Length; i++)
                    {
                        returnList.Add(nativeList[i].ToString());
                    }
                }
            }
            finally
            {
                 // Dispose the NativeList allocated by FindAllWordsInGrid
                if (nativeList.IsCreated)
                {
                    nativeList.Dispose();
                }
            }


            return returnList;
        }

    
        public bool TestifyWord(string word)
        {
            Debug.Log(word);
            //return random value of true or false
            return Random.Range(0, 2) == 1;
        }
    }
}
