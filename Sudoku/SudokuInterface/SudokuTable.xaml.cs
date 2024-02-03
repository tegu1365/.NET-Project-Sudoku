using SudokuReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SudokuInterface
{
    /// <summary>
    /// Interaction logic for SudokuTable.xaml
    /// </summary>
    public partial class SudokuTable : UserControl
    {
        public SudokuTable()
        {
            InitializeComponent();
            InitializeSudokuGrid();
        }
        /// <summary>
        /// Creating empty sudoku grid with every cell having name cell number of row number of col
        /// </summary>
        private void InitializeSudokuGrid()
        {
            Game = new GameSudoku();
            sudokuCells = new TextBox[9, 9];

            for (int i = 0; i < 9; i++)
            {
                sudokuGrid.ColumnDefinitions.Add(new ColumnDefinition());
                sudokuGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox cell = new TextBox();
                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    cell.Name = $"c{row}{col}";
                    sudokuGrid.Children.Add(cell);
                    sudokuCells[row, col] = cell;
                    cell.TextChanged += Cell_TextChanged;
                }
            }

            if(Game != null)
            {
                FillGrid(Game);
            }
        }

        private TextBox[,] sudokuCells;
        private GameSudoku Game;


        /// <summary>
        /// Every representation of the grid in SudokuReader is with List<List<int>> this will fill the whole
        /// grid with solution, game state or initial.
        /// </summary>
        /// <param name="sudoku"></param>
        public void FillGrid(GameSudoku sudoku)
        {
            Game = sudoku;
            ClearCells();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    string name = $"c{row}{col}";

                    if (sudoku.State[row][col] != 0)
                    {
                        TextBox cell = sudokuCells[row, col];
                        cell.Text = sudoku.State[row][col].ToString();
                        if (sudoku.Initial[row][col] != 0) cell.IsEnabled = false;
                    }
                }
            }
        }
        /// <summary>
        /// Validate that the input is a single digit between 1 and 9
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsValidInput(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length == 1 && char.IsDigit(input[0]) && input[0] >= '1' && input[0] <= '9';
        }

        /// <summary>
        /// Validating every number inputed in cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int row = Grid.GetRow(textBox);
            int col = Grid.GetColumn(textBox);
            if (IsValidInput(textBox.Text))
            {
                Game.ChangeCell(row, col, int.Parse(textBox.Text));
            }
            else
            {
                textBox.Text = "";
            }
        }
        /// <summary>
        /// Getting te solution to be displayed
        /// </summary>
        public void Solve()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    string name = $"c{row}{col}";
                    TextBox cell = sudokuCells[row, col];
                    cell.Text = Game.Puzzle.Solution[row][col].ToString();
                    if (Game.Initial[row][col] != 0) cell.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// Save game
        /// </summary>
        public void Save()
        {
            Game.SaveGame();
        }
        /// <summary>
        /// Undo function 
        /// </summary>
        public void Undo()
        {
            var item=Game.Undo();
            if (item != (-1,-1,-1,-1))
            {
                sudokuCells[item.Row,item.Col].Text = Game.State[item.Row][item.Col].ToString();
            }
        }
        /// <summary>
        /// Redo function
        /// </summary>
        public void Redo()
        {
            var item = Game.Redo();
            if (item != (-1, -1, -1, -1))
            {
                sudokuCells[item.Row, item.Col].Text = Game.State[item.Row][item.Col].ToString();
            }
        }

        /// <summary>
        /// Check if puzzle is solved
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
           return Game.CheckSolved();
        }

        /// <summary>
        /// Clearing all cell so we can have clean slate for new game
        /// </summary>
        private void ClearCells()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sudokuCells[row, col].Text = "";
                    sudokuCells[row,col].IsEnabled = true;
                }
            }
        }
    }
}
