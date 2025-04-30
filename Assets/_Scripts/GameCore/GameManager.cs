using System;
using System.Collections.Generic;
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
        
        private List<string> _foundWords = new List<string>();

        #region Public Methods

        public void AddFoundWord(string word) 
        {
            if (string.IsNullOrEmpty(word) || _foundWords.Contains(word)) return;
            _foundWords.Add(word);
            FoundWords.Value++;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
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
            OnGameStart().Forget();
        }

        #endregion

        #region Private Methods

        private async UniTask OnGameStart()
        {
            Timer.Value = gameTimerSeconds;
            FoundWords.Value = 0;

            while (Timer.Value > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                Timer.Value--;
            }

            GameStarted.OnNext(Unit.Default);
        }

        private void OnGameOver()
        {
            GameEnded.OnNext(Unit.Default);
        }

        #endregion

        #region IGameService Members

        public ReactiveProperty<int> FoundWords { get; private set; } = new ReactiveProperty<int>(0);

        public Subject<Unit> GameEnded { get; } = new Subject<Unit>();
        public Subject<Unit> GameStarted { get; } = new Subject<Unit>();

        public ReactiveProperty<int> Timer { get; private set; } = new ReactiveProperty<int>(0);

        #endregion
    }
}
