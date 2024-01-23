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

    public class GameSudoku
    {
        SudokuPuzzle Puzzle { get; set; }
        public List<List<int>> Initial { get; set; }
        public List<List<int>> State { get; set; }
        Level Level { get; set; }

        private List<(int Row, int Col, int oldNumber, int newNumber)> history;
        private int currentTime;

        public GameSudoku(Level level, SudokuPuzzle puzzle)
        {
            Puzzle = puzzle;
            Level = level;
            State = CreateState();
            Initial = State;
            history=new List<(int Row, int Col, int oldNumber, int newNumber)>();
            currentTime=0;
        }

        public GameSudoku()
        {
            ReadFile();
            history = new List<(int Row, int Col, int oldNumber, int newNumber)>();
            currentTime = 0;
        }

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

        private List<List<int>>? CreateState()
        {
            if(Puzzle!=null)
            {
                List < List<int> > newState = new List<List<int>> (Puzzle.Solution);
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

        public void Undo()
        {
            if (currentTime > 0)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime-1];
                State[last.Row][last.Col] = last.oldNumber;
                currentTime--;
            }
        }

        public void Redo()
        {
            if (currentTime<history.Count)
            {
                (int Row, int Col, int oldNumber, int newNumber) last = history[currentTime];
                State[last.Row][last.Col] = last.newNumber;
                currentTime++;
            }
        }

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

        public bool ValidNumber(int row, int col, int num)
        {
            if (num != 0)
            {
                bool validRow=!State[row].Contains(num);

            }
            return true;
        }

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
