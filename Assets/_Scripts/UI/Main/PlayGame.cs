using Interfaces;
using UnityEngine;
using VContainer;

namespace UI.Main
{
    public class PlayGame : MonoBehaviour
    {
        private ISceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(ISceneLoadService sceneLoadService)
        {
            _sceneLoadService = sceneLoadService;
        }
    
        public void OnPlayGamePressed()
        {
            _sceneLoadService.Load(ISceneLoadService.SceneName.GameScene);
        }
    }
}
