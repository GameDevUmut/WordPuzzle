using System;
using DG.Tweening;
using Interfaces;
using R3;
using TMPro;
using UI.Game.InGamePopups;
using UnityEngine;
using VContainer;

namespace UI.Game
{
    public class GameUI : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private FoundWordsPopup foundWordsPopup;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text foundWordsText;

        #endregion

        #region Fields

        private IGameService _gameService;
        private IGridService _gridService;

        private Color _timerOriginalColor, _foundWordsOriginalColor;

        private ITrieService _trieService;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _timerOriginalColor = timerText.color;
            _foundWordsOriginalColor = foundWordsText.color;
        }

        private void Start()
        {
            SubscribeToTimer();
            SubscribeToFoundWords();
        }

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

        private void SubscribeToTimer()
        {
            _gameService.Timer.Subscribe(timer =>
            {
                var timeSpan = TimeSpan.FromSeconds(timer);
                timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

                //red flash
                DOTween.Sequence().Append(timerText.DOColor(Color.red, 0.2f))
                    .Append(timerText.DOColor(_timerOriginalColor, 0.2f)).SetEase(Ease.InOutQuad);
            });
        }

        private void SubscribeToFoundWords()
        {
            _gameService.FoundWords.Subscribe(foundWords =>
            {
                foundWordsText.text = foundWords.ToString();

                if (foundWords > 0)
                {
                    //green flash
                    DOTween.Sequence().Append(foundWordsText.DOColor(Color.green, 0.2f))
                        .Append(foundWordsText.DOColor(_foundWordsOriginalColor, 0.2f)).SetEase(Ease.InOutQuad);
                }
            });
        }

        [Inject]
        private void Construct(ITrieService trieService, IGridService gridService, IGameService gameService)
        {
            _gameService = gameService;
            _gridService = gridService;
            _trieService = trieService;
        }

        #endregion
    }
}
