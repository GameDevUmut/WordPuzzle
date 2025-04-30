using R3;

namespace Interfaces
{
    public interface IGameService
    {
        ReactiveProperty<int> Timer { get; }
        ReactiveProperty<int> FoundWords { get; }
        Subject<Unit> GameEnded { get; }
        Subject<Unit> GameStarted { get; }
        
        void AddFoundWord(string word);
    }
}
