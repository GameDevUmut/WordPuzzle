using Interfaces;
using UI.Game.InGamePopups;
using UnityEngine;
using VContainer;

namespace UI.Game
{
    public class GameUI : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private FoundWordsPopup foundWordsPopup;

        #endregion

        #region Fields

        private ITrieService _trieService;
        private IGridService _gridService;

        #endregion

        #region Public Methods

        public async void OnSearchButtonClick()
        {
            var returnList = await _trieService.SearchPossibleWords();
            foundWordsPopup.ShowFoundWords(returnList);
        }

        public void OnRefillButtonClick()
        {
            _gridService.RecreateGrid();
        }

        #endregion

        #region Private Methods

        [Inject]
        private void Construct(ITrieService trieService, IGridService gridService)
        {
            _gridService = gridService;
            _trieService = trieService;
        }

        #endregion
    }
}
