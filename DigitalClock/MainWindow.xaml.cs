using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            started = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Interval=new TimeSpan(0,0,1);
            timer.Tick += TicTac;
            timer.Start();
        }

        private DispatcherTimer timer;
        private DateTime started;

        private void TicTac(object? sender, EventArgs e)
        {
            var date = DateTime.Now.Subtract(started);
            var hour = date.Hours;
            var minute = date.Minutes;
            var second = date.Seconds;
            txtTime.Text = $"{hour:D2}:{minute:D2}:{second:D2}";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        //private void BtnStop_Click(object sender, RoutedEventArgs e)
        //{
        //    timer.Stop();
        //}

        //private void BtnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    timer.Start();
        //}
    }
}
