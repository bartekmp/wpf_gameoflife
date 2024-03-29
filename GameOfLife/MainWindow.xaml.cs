﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using GameOfLife.Annotations;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        
        Game _gameOfLife;
        private const int CellsWidth = 40, CellsHeight = 20;
        private double? _speed;

        public event PropertyChangedEventHandler PropertyChanged;

        public double? Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (value != null) _gameOfLife.Speed = value.Value;
                OnPropertyChanged("Speed");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _gameOfLife = new Game(50, 30) {CellGrid = MainGrid};
            DataContext = _gameOfLife;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int width, height;
            width = int.TryParse(WidthBox.Text, out width) ? ((width >= 10 && width <= 100) ? width : CellsWidth) : CellsWidth;
            height = int.TryParse(HeightBox.Text, out height) ? ((height >= 10 && height <= 100) ? height : CellsHeight) : CellsHeight;

            _gameOfLife.Speed = _speed ?? 300;
            _gameOfLife.ResizeGrid(width, height);
            _gameOfLife.PopulateGrid();
             
            _gameOfLife.GameStarted = true;
            _gameOfLife.GameNotStarted = false;

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
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".cell",
                Filter = "Game of Life board (*.cell)|*.cell"
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
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".cell",
                Filter = "Game of Life board (*.cell)|*.cell"
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
            _gameOfLife.Pause = false;
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _gameOfLife.Pause = !_gameOfLife.Pause;
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
