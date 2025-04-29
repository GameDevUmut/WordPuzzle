using R3;

namespace Interfaces
{
    public interface IGridService
    {
        public int GridRows { get; }
        public int GridColumns { get; }
        public char GetCellCharacter(int row, int column);
        
        public Observable<Unit> GridCreated { get; }
    }
}
