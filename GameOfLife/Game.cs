using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class Game : INotifyPropertyChanged
    {
        private int _iterations;
        private int _stepCounter;

        private const int GridWidth = 50;
        private const int GridHeight = 30;

        public Cell[,] Cells = new Cell[GridHeight, GridWidth];
        private Grid _grid;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public int StepCounter
        {
            get
            {
                return _stepCounter;
            }

            set
            {
                if (_stepCounter != value)
                {
                    _stepCounter = value;
                    OnPropertyChanged("StepCounter");
                }
            }
        }

        public Game()
        {
            _dispatcherTimer.Tick += Step;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 500);
        }

        public void PopulateGrid(Grid mainGrid)
        {
            _grid = mainGrid;
            for (var i = 0; i < GridWidth; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var i = 0; i < GridHeight; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    var cell = new Cell(i, j);
                    cell.Btn.Click += ClickCell;
                    Cells[j, i] = cell;


                    Grid.SetColumn(cell.Rect, i);
                    Grid.SetRow(cell.Rect, j);
                    mainGrid.Children.Add(cell.Rect);

                    Grid.SetColumn(cell.Btn, i);
                    Grid.SetRow(cell.Btn, j);
                    mainGrid.Children.Add(cell.Btn);

                }
            }
        }

        public void RunSimulation(int iterations)
        {
            _iterations = iterations;
            _dispatcherTimer.Start();
        }

        public void Step(object sender, EventArgs e)
        {
            _iterations--;
            if (_iterations <= 0)
            {
                _dispatcherTimer.Stop();
                return;
            }

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    var living = Cells[j, i].IsAlive;
                    var count = GetLivingNeighbors(i, j);
                    var result = false;
                    
                    if (living && count < 2)
                        result = false;
                    if (living && (count == 2 || count == 3))
                        result = true;
                    if (living && count > 3)
                        result = false;
                    if (!living && count == 3)
                        result = true;

                    if (living != result)
                        Cells[j, i].SetState(result);
                }
            }
            StepCounter++;
        }

        private int GetLivingNeighbors(int x, int y)
        {
            var count = 0;
            // Cells[y, x]
            // y - wiersz
            // x - kolumna
            
            // cell on the right
            if (x < GridWidth - 1)
                if (Cells[y, x+1].IsAlive)
                    count++;

            // cell on the bottom right
            if (x < GridWidth - 1 && y < GridHeight - 1)
                if (Cells[y + 1, x + 1].IsAlive)
                    count++;

            // cell on the bottom
            if (y < GridHeight - 1)
                if (Cells[y + 1, x].IsAlive)
                    count++;

            // cell on the bottom left
            if (x > 0 && y < GridHeight - 1)
                if (Cells[y +1, x -1].IsAlive)
                    count++;

            //  cell on the left
            if (x > 0)
                if (Cells[y, x-1].IsAlive)
                    count++;

            // cell on the top left
            if (x > 0 && y > 0)
                if (Cells[y - 1, x - 1].IsAlive)
                    count++;

            // cell on the top
            if (y > 0)
                if (Cells[y-1, x].IsAlive)
                    count++;

            // cell on the top right
            if (x < GridWidth - 1 && y > 0)
                if (Cells[y - 1, x + 1].IsAlive)
                    count++;

            return count;
        }
        private void ClickCell(object sender, RoutedEventArgs e)
        {
            var element = (UIElement)e.Source;

            var c = Grid.GetColumn(element);
            var r = Grid.GetRow(element);

            Cells[r, c].ChangeState();
        }

        public void ResetGame()
        {
            _dispatcherTimer.Stop();
            _iterations = 0;
            StepCounter = 0;

            for (var i = 0; i < GridWidth; i++)
            {
                for (var j = 0; j < GridHeight; j++)
                {
                    Cells[j, i].SetDead();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
