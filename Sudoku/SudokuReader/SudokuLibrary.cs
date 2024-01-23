using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SudokuReader
{
    public static class SudokuLibrary
    {
        private static List<SudokuPuzzle> sudokuPuzzles=ReadSudoku();
        private static XmlReader xmlReader;
        private const string link = "..\\..\\..\\xml\\sudoku.xml";

        private static string GetExecutingDirectory()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location);
        }

        /// <summary>
        /// Reads the xml file and creates list with all puzzles in file
        /// </summary>
        /// <returns></returns>
        private static List<SudokuPuzzle> ReadSudoku()
        {
            List<SudokuPuzzle> puzzles = new List<SudokuPuzzle>();
            string fullPath = Path.Combine(GetExecutingDirectory(), link);
            xmlReader = XmlReader.Create(fullPath);
            xmlReader = XmlReader.Create(link);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "SudokuPuzzle")
                {
                    int puzzleId = int.Parse(xmlReader.GetAttribute("id"));

                    SudokuPuzzle puzzle = new SudokuPuzzle
                    {
                        Id = puzzleId,
                        Solution = new List<List<int>>()
                    };

                    xmlReader.ReadToFollowing("Solution");

                    while (xmlReader.ReadToFollowing("row"))
                    {
                        string[] numbers = xmlReader.ReadElementContentAsString().Split(',');
                        List<int> current = new List<int>();
                        for (int col = 0; col < numbers.Length; col++)
                        {
                            current.Add(int.Parse(numbers[col]));
                        }
                        puzzle.Solution.Add(current);
                        if(puzzle.Solution.Count==9) { break; }
                    }

                    puzzles.Add(puzzle);
                }
            }

            return puzzles;
        }

        public static SudokuPuzzle GetPuzzle(int id)
        {
            return sudokuPuzzles.Find(puzzle => puzzle.Id == id);
        }

        public static string ToString()
        {
            string str = "";
            foreach (var puzzle in sudokuPuzzles)
            {
                str += $"ID: {puzzle.Id}\n";
                str += "Solution:\n";
                str += puzzle;
            }
            return str;
        }
    }
}
