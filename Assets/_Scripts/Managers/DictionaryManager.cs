using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using IDictionaryService = Interfaces.IDictionaryService;

namespace Managers
{
    public class DictionaryManager : MonoBehaviour, IDictionaryService
    {
        #region Serializable Fields

        [SerializeField] private string language = "Turkish";

        #endregion

        #region Fields

        private UniTaskCompletionSource<bool> _tcs = new UniTaskCompletionSource<bool>();

        #endregion

        #region Properties

        public UniTask<bool> DictionaryLoadTask => _tcs.Task;

        public List<string> LoadedWords { get; private set; } = new List<string>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            LoadAddressableDictionary();
        }

        #endregion

        #region Private Methods

        private async UniTask LoadAddressableDictionary()
        {
            try
            {
                string addressableKey = $"Dictionary_{language}";
                var textAsset = await Addressables.LoadAssetAsync<TextAsset>(addressableKey);

                if (textAsset == null)
                {
                    Debug.LogError($"Failed to load dictionary: {addressableKey}");
                    _tcs.TrySetResult(false);
                    return;
                }

                string jsonContent = textAsset.text;
                Addressables.Release(textAsset);

                LoadedWords = await Task.Run(() =>
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonContent);
                    return data != null && data.TryGetValue("words", out var words) ? words : new List<string>();
                });

                _tcs.TrySetResult(true);
                Debug.Log($"{language} dictionary loaded successfully with {LoadedWords.Count} words.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading dictionary: {ex.Message}");
                _tcs.TrySetException(ex);
            }
        }

        #endregion
    }
}
