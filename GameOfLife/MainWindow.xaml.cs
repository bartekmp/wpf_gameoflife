using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        Game _gameOfLife = new Game();
        public bool GameStarted;
        public MainWindow()
        {
            InitializeComponent();
            _gameOfLife.PopulateGrid(MainGrid);
            DataContext = _gameOfLife;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (CountBox.Text == string.Empty)
            {
                GameStarted = true;
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
                GameStarted = true;
                _gameOfLife.RunSimulation(iterations);
            }
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            if(GameStarted)
                _gameOfLife.Step(this, null);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _gameOfLife.ResetGame();
        }
    }

    public static class Helper
    {
        public static void Refresh(this UIElement uiElement)
        {
            //uiElement.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
        }
    }
}
