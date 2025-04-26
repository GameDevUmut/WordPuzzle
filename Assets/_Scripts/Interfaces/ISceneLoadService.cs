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
        public UniTask Load(SceneName sceneName);
        
        public UniTask Load(string sceneName, bool unloadLast = false);
        
        public UniTask UnloadLast();
        
        public UniTask UnloadLastWithCount(int i);

        public void ToggleLoadingScreen(bool state);
        public void ToggleSplashScreen(bool state);
        
    }
}
