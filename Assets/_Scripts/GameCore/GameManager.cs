using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Interfaces;
using R3;
using UnityEngine;

namespace GameCore
{
    public class GameManager : MonoBehaviour, IGameService
    {
        #region Serializable Fields

        [SerializeField] private int gameTimerSeconds = 90;

        #endregion

        #region Fields

        private List<string> _foundWords = new List<string>();
        private CancellationTokenSource _tokenSource;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _tokenSource = new CancellationTokenSource();
            Timer.Value = gameTimerSeconds;
            Timer.Subscribe(value =>
            {
                if (value <= 0)
                {
                    OnGameOver();
                }
            });
        }

        private void Start()
        {
            OnGameStart(_tokenSource.Token).Forget();
        }

        private void OnDestroy()
        {
            _tokenSource?.Cancel();
        }

        #endregion

        #region Private Methods

        private async UniTask OnGameStart(CancellationToken token)
        {
            Timer.Value = gameTimerSeconds;
            FoundWords.Value = 0;
            GameStarted.OnNext(Unit.Default);

            while (Timer.Value > 0 && !_tokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                    Timer.Value--;
                }
                catch (OperationCanceledException e)
                {
                    //on cancel
                    Debug.Log("Timer cancelled");
                    break;
                }
            }
        }

        private void OnGameOver()
        {
            GameEnded.OnNext(Unit.Default);
        }

        #endregion

        #region IGameService Members

        public void AddFoundWord(string word)
        {
            if (string.IsNullOrEmpty(word) || _foundWords.Contains(word)) return;
            _foundWords.Add(word);
            FoundWords.Value++;
        }

        public ReactiveProperty<int> FoundWords { get; private set; } = new ReactiveProperty<int>(0);

        public Subject<Unit> GameEnded { get; } = new Subject<Unit>();
        public Subject<Unit> GameStarted { get; } = new Subject<Unit>();

        public ReactiveProperty<int> Timer { get; private set; } = new ReactiveProperty<int>(0);

        #endregion
    }
}
