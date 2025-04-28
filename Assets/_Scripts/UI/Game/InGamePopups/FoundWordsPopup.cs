using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game.InGamePopups
{
    public class FoundWordsPopup : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private GameObject foundWordPrefab;
        [SerializeField] private Transform foundWordsParent;

        #endregion

        #region Public Methods

        public async void ShowFoundWords(List<string> foundWords)
        {
            ClearFoundWords();
            foreach (var word in foundWords)
            {
                AddFoundWord(word);
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        private void ClearFoundWords()
        {
            foreach (Transform child in foundWordsParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void AddFoundWord(string word)
        {
            var foundWord = Instantiate(foundWordPrefab, foundWordsParent);
            foundWord.GetComponentInChildren<TMP_Text>().text = word;
        }

        #endregion
    }
}
