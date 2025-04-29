using R3;

namespace Interfaces
{
    public interface IGridService
    {
        int GridRows { get; }
        int GridColumns { get; }
        char GetCellCharacter(int row, int column);

        void ReCreateGrid();
        
        Observable<Unit> GridCreated { get; }
    }
}
