
namespace SudokuReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SudokuLibrary.GetPuzzle(1);
           // Console.WriteLine(a);

            GameSudoku gameSudoku = new GameSudoku(Level.Easy, SudokuLibrary.GetPuzzle(1));
            Console.WriteLine(gameSudoku);
      
            gameSudoku.SaveGame();
        }
    }
}