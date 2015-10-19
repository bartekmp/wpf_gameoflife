using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using GameOfLife.Annotations;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        Game _gameOfLife;
        private const int CellsWidth = 40, CellsHeight = 20;
        
        public MainWindow()
        {
            InitializeComponent();

            _gameOfLife = new Game(50, 30);
            DataContext = _gameOfLife;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int width, height;
            width = int.TryParse(WidthBox.Text, out width) ? ((width >= 10 && width <= 100) ? width : CellsWidth) : CellsWidth;
            height = int.TryParse(HeightBox.Text, out height) ? ((height >= 10 && height <= 100) ? height : CellsHeight) : CellsHeight;

            _gameOfLife.ResizeGrid(width, height);
            _gameOfLife.PopulateGrid(MainGrid);

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

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
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
    }

    public static class Helper
    {
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
        }
    }
}
