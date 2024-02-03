using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SudokuReader
{
    public enum Level
    {
        Easy,
        Medium,
        Hard
    }
    /// <summary>
    /// The class for sudoku game
    /// </summary>
    public class GameSudoku
    {
        /// <summary>
        /// has the puzzle from the library
        /// </summary>
        public SudokuPuzzle Puzzle { get; set; }
        /// <summary>
        /// the initial state when game is created
        /// </summary>
        public List<List<int>> Initial { get; set; }
        /// <summary>
        /// The current state of the game
        /// </summary>
        public List<List<int>> State { get; set; }
        /// <summary>
        /// the level of hardness
        /// </summary>
        Level Level { get; set; }
        /// <summary>
        /// history of all moves
        /// </summary>
        private List<(int Row, int Col, int oldNumber, int newNumber)> history;
        /// <summary>
        /// witch move is the current move
        /// </summary>
        private int currentTime;

        /// <summary>
        /// basic function to clone list of list
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<List<int>> ClonePuzzle(List<List<int>> source)
        {
            return source.Select(row => new List<int>(row)).ToList();
        }

        /// <summary>
        /// creating new game
        /// </summary>
        /// <param name="level"></param>
        /// <param name="puzzle"></param>

        public GameSudoku(Level level, SudokuPuzzle puzzle)
        {
            Puzzle = new SudokuPuzzle
            {
                Id = puzzle.Id,
                Solution = ClonePuzzle(puzzle.Solution)
            };
            Level = level;
            State = CreateState();
            Initial = ClonePuzzle(State);
            history=new List<(int Row, int Col, int oldNumber, int newNumber)>();
            currentTime=0;
        }

        /// <summary>
        /// reading history file and init the game
        /// </summary>
        public GameSudoku()
        {
            ReadFile();
            history = new List<(int Row, int Col, int oldNumber, int newNumber)>();
            currentTime = 0;
        }
        /// <summary>
        /// Basic function for creating game from file
        /// </summary>
        private void ReadFile()
        {
            History loadedHistory = History.LoadFromXml("history1.xml");

            Puzzle = new SudokuPuzzle
            {
                Id = loadedHistory.Puzzles[0].Id,
                Solution = loadedHistory.Puzzles[0].Solution
            };

            Initial = loadedHistory.Puzzles[0].Initial;
            State = loadedHistory.Puzzles[0].State;
            Level = loadedHistory.Puzzles[0].Level;

        }

        /// <summary>
        /// Save game to file
        /// </summary>
        public void SaveGame()
        {
            History xmlHistory = new History
            {
                Puzzles = new List<Sudoku>
                {
                    new Sudoku
                    {
                        Id=this.Puzzle.Id,
                        Level=this.Level,
                        Initial=this.Initial,
                        Solution=this.Puzzle.Solution,
                        State=this.State
                    }
                }
            };
            xmlHistory.SaveToXml("history1.xml");
        }

        /// <summary>
        /// Initializing state for new game
        /// </summary>
        /// <returns></returns>
        private List<List<int>>? CreateState()
        {
            if(Puzzle!=null)
            {
                List <List<int> > newState = new List<List<int>> ();
                newState = ClonePuzzle(Puzzle.Solution);
                int numberMissing = Level switch
                {
                    Level.Easy => 40,
                    Level.Medium => 60,
                    Level.Hard => 70,
                    _=>0
                };

                Random rnd= new Random();

                for(int i = 0; i < numberMissing; i++)
                {
                    int rndRow=rnd.Next(0,9);
                    int rndCol=rnd.Next(0,9);
                    newState[rndRow][rndCol] = 0;
                }
                return newState;
            }
            return new List<List<int>>();
        }

        /// <summary>
        /// Changing the number in the cell with coord row and col.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="num"></param>
        public void ChangeCell(int row, int col, int num)
        {
            if (Initial[row][col] == 0)
            {
                int oldNum = State[row][col];
                if (currentTime != history.Count)
                {
                    history.RemoveRange(currentTime, history.Count - currentTime);
                }

                history.Add((row, col, oldNum, num));
                currentTime = history.Count;
                State[row][col] = num;
            }
            
        }

        /// <summary>
        /// Undo last change in cell in the timeline
        /// </summary>
        /// <returns></returns>
        public (int Row, int Col, int oldNumber, int newNumber) Undo()
        {
            if (currentTime > 0)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime-1];
                State[last.Row][last.Col] = last.oldNumber;
                currentTime--;
                return last;
            }
            return (-1, -1, -1, -1);
        }
        /// <summary>
        /// Redo the last change in cell in the timeline
        /// </summary>
        /// <returns></returns>
        public (int Row, int Col, int oldNumber, int newNumber) Redo()
        {
            if (currentTime<history.Count)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime];
                State[last.Row][last.Col] = last.newNumber;
                currentTime++;
                return last;
            }
            return (-1, -1, -1, -1);
        }

        /// <summary>
        /// Check if the state is solution
        /// </summary>
        /// <returns></returns>
        public bool CheckSolved()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (Puzzle.Solution[i][j] != State[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// ToString function for testing.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            foreach (var row in State)
            {
                foreach (var num in row)
                {
                    str += num + " ";
                }
                str += "\n";
            }
            if (currentTime > 0)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime - 1];
                str += $"Undo: [{last.Row},{last.Col}] ={last.oldNumber}";
            }
            if (currentTime < history.Count)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime];
                str += $"Redo: [{last.Row},{last.Col}] ={last.newNumber}";
            }
            return str;
        }
    }
}
