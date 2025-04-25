using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using Task = System.Threading.Tasks.Task;


namespace Managers
{
    public class SceneLoadManager : MonoBehaviour, ISceneLoadService
    {
        #region Serializable Fields

        [Header("Addressable Scenes")]
        [SerializeField] private AssetReference mainScene;
        [SerializeField] private AssetReference gameScene;

        [SerializeField] private GameObject loadingScreenObject;
        [SerializeField] private GameObject splashScreenObject;
        [SerializeField] private Image splashScreenLoadingBarFill;
        [SerializeField] private Image loadingScreenLoadingBarFill;
        

        #endregion

        #region Fields

        // private IAnalyticsService _iAnalyticsService;

        public SceneInstance LastScfeneInstance => _sceneStack.Peek();
        private Stack<SceneInstance> _sceneStack = new Stack<SceneInstance>();
        private DateTime _splashScreenOpenedTime;
        private ISceneLoadService _sceneLoadServiceImplementation;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Unity Methods

        private async void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _splashScreenOpenedTime = DateTime.Now;
            LogSplashScreenOpened();

            await UniTask.Delay(100);
            
            LoadMainScene();
        }

        // [Inject]
        // private void Initialize(IAnalyticsService iAnalyticsService)
        // {
        //     _iAnalyticsService = iAnalyticsService;
        // }

        #endregion

        #region Private Methods

   

        private async UniTask DirectlyPlayGame()
        {
            ToggleSplashScreen(true);

            // Start at 0
            FillLoadingBar(0f);

            var gameLoader = Addressables.LoadSceneAsync(gameScene, LoadSceneMode.Additive, false);
            
            UpdateProgressBar(gameLoader, 0f, 0.3f, _cancellationTokenSource.Token);

            _sceneStack.Push(await gameLoader);
            FillLoadingBar(0.5f);
            gameLoader.Result.ActivateAsync();
            LogSplashScreenClosed();
        }

        private async Task UpdateProgressBar(AsyncOperationHandle<SceneInstance> gameLoader, float startProgress = 0f, float speed = 0.5f, CancellationToken cancellationToken = default)
        {
            float progress = startProgress;
          while (!gameLoader.IsDone && (cancellationToken == default || !cancellationToken.IsCancellationRequested))
            {
                progress = Mathf.MoveTowards(progress, 1f, Time.deltaTime * speed);
                FillLoadingBar(progress);
                await UniTask.Yield();
            }
        }

        #region Event Logging

        private void LogSplashScreenClosed()
        {
            var timeSpent = DateTime.Now - _splashScreenOpenedTime;
            
        }

        private void LogSplashScreenOpened()
        {
          
        }

        #endregion

        private async UniTask LoadMainScene()
        {
            ToggleSplashScreen(true);
            
            var mainLoader = Addressables.LoadSceneAsync(mainScene, LoadSceneMode.Additive);
            _sceneStack.Push(await mainLoader);
            
            ToggleSplashScreen(false);
        }

        private async UniTask LoadGameScene()
        {
            await UnloadLast();
            var gameLoader = Addressables.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
            _sceneStack.Push(await gameLoader);
        }

        #endregion

        #region ISceneLoadService Members

        public async UniTask Load(string sceneName, bool unloadLast = false)
        {
            if (unloadLast)
                await UnloadLast();

            var sceneLoader = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive, false);
            
            if(_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
            
            UpdateProgressBar(sceneLoader, 0.5f, 1f);
            
            var sceneInstance = await sceneLoader;
            _sceneStack.Push(sceneInstance);
            
            FillLoadingBar(1f);
            await sceneLoader.Result.ActivateAsync();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        public async UniTask UnloadLast()
        {
            await Addressables.UnloadSceneAsync(_sceneStack.Pop(), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        public async UniTask UnloadLastWithCount(int i)
        {
            for (var j = 0; j < i; j++)
            {
                await UnloadLast();
            }
        }

        public void ToggleLoadingScreen(bool state)
        {
            // if(!state)
            //     loadingScreenLoadingBarFill.fillAmount = 0f;
            
            loadingScreenObject.SetActive(state);
        }

        public void ToggleSplashScreen(bool state)
        {
            // if(!state)
            //     splashScreenLoadingBarFill.fillAmount = 0f;
            
            splashScreenObject.SetActive(state);
        }

        public void FillLoadingBar(float value)
        {
            return; //To be removed
            if(splashScreenObject.activeSelf)
                splashScreenLoadingBarFill.fillAmount = value;
            else if(loadingScreenObject.activeSelf)
                loadingScreenLoadingBarFill.fillAmount = value;
        }

        public async UniTask Load(ISceneLoadService.SceneName sceneName)
        {
            switch (sceneName)
            {
                case ISceneLoadService.SceneName.MainScene:
                    await LoadMainScene();
                    break;
                case ISceneLoadService.SceneName.GameScene:
                    await LoadGameScene();
                    break;
            }
        }
        #endregion
    }
}
