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
using System.Windows.Threading;

namespace DigitalClock
{
    /// <summary>
    /// Interaction logic for Timer.xaml
    /// </summary>
    public partial class Timer : UserControl
    {
        /// <summary>
        /// Based on the digital clock from the exercises 
        /// </summary>
        public Timer()
        {
            InitializeComponent();
            
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TicTac;
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

        public void Start()
        {
            started = DateTime.Now;

            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
       
        /// <summary>
        /// I did it this way so I can't accidentally set the timer wrong. 
        /// This is if i want to save the timer in history file.
        /// </summary>
        /// <param name="started"></param>
        public void SetStarted(DateTime started)
        {
            this.started = started;
        }

        public DateTime GetStarted()
        {
            return started;
        }
    }
}
