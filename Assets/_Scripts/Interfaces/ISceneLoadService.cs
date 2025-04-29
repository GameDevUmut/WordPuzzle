using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface ISceneLoadService
    {
        public enum SceneName
        {
            SplashScene,
            MainScene,
            GameScene
        }
        UniTask Load(SceneName sceneName);
        
        UniTask Load(string sceneName, bool unloadLast = false);
        
        UniTask UnloadLast();
        
        UniTask UnloadLastWithCount(int i);

        void ToggleLoadingScreen(bool state);
        void ToggleSplashScreen(bool state);
        
    }
}
