using GameCore.GridSystem;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Main
{
    public class PlayGame : MonoBehaviour
    {
        [SerializeField] private TMP_InputField xInputField;
        [SerializeField] private TMP_InputField yInputField;
        
        
        private ISceneLoadService _sceneLoadService;

        [Inject]
        public void Construct(ISceneLoadService sceneLoadService)
        {
            _sceneLoadService = sceneLoadService;
        }
        
        private Vector2 GetInputvalues()
        {
            int x = int.Parse(xInputField.text);
            int y = int.Parse(yInputField.text);
            return new Vector2(x, y);
        }
    
        public void OnPlayGamePressed()
        {
            GridManager.GridSize = GetInputvalues();
            _sceneLoadService.Load(ISceneLoadService.SceneName.GameScene);
        }
    }
}
