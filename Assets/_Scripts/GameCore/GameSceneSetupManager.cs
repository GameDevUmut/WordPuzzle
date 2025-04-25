using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public class GameSceneSetupManager : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private string[] maps;

        #endregion

        #region Fields

        private UniTask _task;
        private ISceneLoadService _sceneLoadService;

        #endregion

        #region Properties

        public UniTaskCompletionSource<bool> SceneLoadTaskCompletionSource { get; private set; } = new UniTaskCompletionSource<bool>();

        #endregion

        #region Unity Methods

        [Inject]
        private async void Init(ISceneLoadService sceneLoadService)
        {
            _sceneLoadService = sceneLoadService;
            //await _sceneLoadService.Load(maps[0], false);
            _sceneLoadService.ToggleLoadingScreen(false);
            _sceneLoadService.ToggleSplashScreen(false);
            if(canvasGroup) canvasGroup.alpha = 1;
            SceneLoadTaskCompletionSource.TrySetResult(true);
            await Task.Delay(900);
        }

        public void RestartLevel()
        {
            _sceneLoadService.UnloadLastWithCount(2);
        }
        
        #endregion
    }
}
