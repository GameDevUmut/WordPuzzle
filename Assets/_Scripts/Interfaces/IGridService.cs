namespace Interfaces
{
    public interface IGridService
    {
        public int GridRows { get; }
        public int GridColumns { get; }
        public char GetCellCharacter(int row, int column);
        public bool TestifyWord(string word);
    }
}
