using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using GameOfLife.Annotations;
using Microsoft.Win32;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        readonly Game _gameOfLife;
        private double? _speed;

        public event PropertyChangedEventHandler PropertyChanged;

        public double? Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (value != null) _gameOfLife.Speed = value.Value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

  

            var w = new SizeWindow();
            w.ShowDialog();
            _gameOfLife = new Game(w.GameWidth, w.GameHeight) { CellGrid = MainGrid };
            DataContext = _gameOfLife;

            _gameOfLife.Speed = _speed ?? 300;
            _gameOfLife.PopulateGrid();

        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            _gameOfLife.Step();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _gameOfLife.ResetGame();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var state = _gameOfLife.SaveState();

            string filename;
            var dlg = new SaveFileDialog
            {
                DefaultExt = ".gol",
                Filter = "Game of Life board (*.gol)|*.gol"
            };
            
            var result = dlg.ShowDialog();

            
            if (result == true)
            {
                filename = dlg.FileName;
            }
            else
            {
                MessageBox.Show("Error while opening the file!");
                return;
            }
            using (var fs = new StreamWriter(filename))
            {
                foreach (var line in state)
                {
                    fs.WriteLine(line);
                }
            }

        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".gol",
                Filter = "Game of Life board (*.gol)|*.gol"
            };

            var result = dlg.ShowDialog();


            if (result == true)
            {
                filename = dlg.FileName;
            }
            else
            {
                MessageBox.Show("Error while opening the file!");
                return;
            }
            var state = File.ReadAllLines(filename);
            _gameOfLife.RestoreState(state);

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenSpeedDialog(object sender, RoutedEventArgs e)
        {
            var w = new SpeedWindow();
            w.ShowDialog();
            Speed = w.SliderValue;
        
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (CountBox.Text == string.Empty)
            {
                _gameOfLife.RunSimulation(1);
            }
            else
            {
                int iterations;
                var successfulParsing = int.TryParse(CountBox.Text, out iterations);
                if (!successfulParsing)
                {
                    MessageBox.Show("Bad value entered!");
                    return;
                }
                _gameOfLife.RunSimulation(iterations);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Helper
    {
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
        }
    }
}
