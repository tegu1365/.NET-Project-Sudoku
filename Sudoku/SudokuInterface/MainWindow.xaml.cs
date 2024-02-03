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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            library = new SudokuLibrary();
            InitializeComponent();
            Timer.Start();
        }

        private SudokuLibrary library;
        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            BtnCheckSolve.IsEnabled = true;
            int level = cmbLevel.SelectedIndex;
            if (level >=0)
            {
                Level lv = level switch
                {
                    1 => Level.Easy,
                    2 => Level.Medium,
                    3 => Level.Hard,
                    _ => Level.Easy
                };
                Random rnd= new Random();
                int id = rnd.Next(1, library.Size());
                GameSudoku current = new GameSudoku(lv,library.GetPuzzle(id));
                SudokuTable.FillGrid(current);
            }
            Timer.Start();
        }

        private void BtnSolve_Click(object sender, RoutedEventArgs e)
        {
            SudokuTable.Solve();
            BtnCheckSolve.IsEnabled = false;
            Timer.Stop();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SudokuTable.Save();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            SudokuTable.Undo();
        }

        private void BtnRedo_Click(object sender, RoutedEventArgs e)
        {
            SudokuTable.Redo();
        }

        private void BtnCheckSolve_Click(object sender, RoutedEventArgs e)
        {
            if (SudokuTable.Check())
            {
                Timer.Stop();
                MessageBox.Show("Congratulations...You Solved it!!!!");
            }
            else
            {
                MessageBox.Show("No no no you have mistakes...");
            }
        }
    }
}
