using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SudokuReader
{
    /// <summary>
    /// Sudoku Puzzle class for basic puzzle type.
    /// </summary>
    public class SudokuPuzzle
    {
        public int Id { get; set; }
        public List<List<int>> Solution { get; set; }
        
        public override string ToString()
        {
            string str = "";
            foreach (var row in Solution)
            {
                foreach (var num in row)
                {
                    str += num + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
