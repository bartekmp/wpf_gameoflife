using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class Game : INotifyPropertyChanged
    {
        private int _iterations;
        private int _stepCounter;

        private bool _gameStarted;

        public double Speed;
        public bool GameStarted
        {
            get { return _gameStarted; }
            set
            {
                _gameStarted = value;
                OnPropertyChanged();
            }
        }

        public bool GameNotStarted
        {
            get { return !_gameStarted; }
            set
            {
                OnPropertyChanged();
            }
        }

        private int _gridWidth;
        private int _gridHeight;

        public CellControl[,] Cells;
        public Grid CellGrid;

        public int StepCounter
        {
            get
            {
                return _stepCounter;
            }

            set
            {
                if (_stepCounter == value) return;
                _stepCounter = value;
                OnPropertyChanged();
            }
        }

        public Game(int width, int height)
        {
            _gridHeight = height;
            _gridWidth = width;
            Cells = new CellControl[_gridHeight, _gridWidth];
        }

        public void ResizeGrid(int width, int height)
        {
            _gridHeight = height;
            _gridWidth = width;
            Cells = new CellControl[_gridHeight, _gridWidth];
        }

        public void PopulateGrid()
        {
            for (var i = 0; i < _gridWidth; i++)
            {
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (var i = 0; i < _gridHeight; i++)
            {
                CellGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (var j = 0; j < _gridHeight; j++)
            {
                for (var i = 0; i < _gridWidth; i++)
                {
                    var cell = new CellControl();
                    Cells[j, i] = cell;
                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);
                    CellGrid.Children.Add(cell);
                    
                }
            }
        }

        public void RunSimulation(int iterations)
        {
            _iterations = iterations;
            while (_iterations-- > 0)
            {
                Step();
                CellGrid.Refresh();
                Thread.Sleep((int)Speed);
            }
        }

        public void Step()
        {
            for (var j = 0; j < _gridHeight; j++)
            {
                for (var i = 0; i < _gridWidth; i++)
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
            
            // cell on the right
            if (x < _gridWidth - 1)
                if (Cells[y, x+1].IsAlive)
                    count++;

            // cell on the bottom right
            if (x < _gridWidth - 1 && y < _gridHeight - 1)
                if (Cells[y + 1, x + 1].IsAlive)
                    count++;

            // cell on the bottom
            if (y < _gridHeight - 1)
                if (Cells[y + 1, x].IsAlive)
                    count++;

            // cell on the bottom left
            if (x > 0 && y < _gridHeight - 1)
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
            if (x < _gridWidth - 1 && y > 0)
                if (Cells[y - 1, x + 1].IsAlive)
                    count++;

            return count;
        }
        public void ClickCell(object sender, RoutedEventArgs e)
        {
            var element = (UIElement)e.Source;

            var c = Grid.GetColumn(element);
            var r = Grid.GetRow(element);

            Cells[r, c].ChangeState();
        }

        public void ResetGame()
        {
            _iterations = 0;
            StepCounter = 0;

            for (var j = 0; j < _gridHeight; j++)
            {
                for (var i = 0; i < _gridWidth; i++)
                {
                    Cells[j, i].Used = false;
                    Cells[j, i].SetDead();
                }
            }
        }

        public string[] SaveState()
        {
            var state = new string[_gridHeight];
            for (var i = 0; i < _gridHeight; i++)
            {
                var sb = new StringBuilder();
                for (var j = 0; j < _gridWidth; j++)
                {
                    var cell = new char[1];
                    if (Cells[i, j].IsAlive)
                    {
                        cell[0] = 'A';
                    }
                    else if (Cells[i, j].Used)
                    {
                        cell[0] = 'U';
                    }
                    else
                    {
                        cell[0] = 'D';
                    }
                    sb.Append(cell);
                }
                state[i] = sb.ToString();
            }
            return state;
        }

        public void RestoreState(string[] state)
        {
            if (GameStarted)
            {
                ResetGame();
            } 

            var height = state.Length;
            var length = state[0].Length;

            if (_gridHeight != height || _gridWidth != length)
            {
                ResizeGrid(length, height);
            }

            if (GameNotStarted)
            {
                PopulateGrid();
            }

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    switch (state[i][j])
                    {
                        case 'A':
                            Cells[i, j].SetAlive();
                            break;
                        case 'U':
                            Cells[i, j].Used = true;
                            Cells[i, j].SetDead();
                            break;
                        default:
                            Cells[i, j].SetDead();
                            break;
                    }
                }
            }
            GameStarted = true;
            GameNotStarted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
