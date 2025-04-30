using Interfaces;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.Game.InGamePopups
{
    public class GameOverPopup : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private TMP_Text foundWordsText;

        #endregion

        #region Fields

        private IGameService _gameService;

        private ISceneLoadService _sceneLoadService;

        #endregion

        #region Public Methods

        public void ShowPopup()
        {
            gameObject.SetActive(true);
            foundWordsText.text = _gameService.FoundWords.Value.ToString();
        }

        public void OnMainMenuButtonClick()
        {
            _sceneLoadService.UnloadLast();
            _sceneLoadService.Load(ISceneLoadService.SceneName.MainScene);
        }

        #endregion

        #region Private Methods

        [Inject]
        private void Construct(ISceneLoadService sceneLoadService, IGameService gameService)
        {
            _gameService = gameService;
            _sceneLoadService = sceneLoadService;
        }

        #endregion
    }
}
