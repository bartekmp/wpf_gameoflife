using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class Cell : INotifyPropertyChanged
    {
        private Cell _newState;
        public bool IsAlive { get; set; }
        public bool Used { get; set; }
        public Brush FillBrush
        {
            get { return _fillBrush; }
            set
            {
                _fillBrush = value;
                OnPropertyChanged();
            }
        }

        private Brush _fillBrush;
        public Cell()
        {
            IsAlive = false;
            Used = false;
            FillBrush = Brushes.Transparent;
            _newState = new Cell(true);

        }

        public void SetAlive()
        {
            _newState.IsAlive = true;
            _newState.Used = true;
            _newState.FillBrush = Brushes.DarkRed;
        }

        public void SetDead()
        {
            _newState.IsAlive = false;
            _newState.FillBrush = Used ? Brushes.YellowGreen : Brushes.Transparent;
        }

        public void ChangeState()
        {
            if (IsAlive)
            {
                SetDead();
            }
            else
            {
                SetAlive();
            }
        }

        public void ChangeCurrentState()
        {
            if (IsAlive)
            {
                IsAlive = false;
                FillBrush = Used ? Brushes.YellowGreen : Brushes.Transparent;
            }
            else
            {
                IsAlive = true;
                Used = true;
                FillBrush = Brushes.DarkRed;
            }
        }

        public void SetCurrentDead()
        {
            IsAlive = false;
            Used = false;
            FillBrush = Used ? Brushes.YellowGreen : Brushes.Transparent;
        }

        public void SetCurrentAlive()
        {
            IsAlive = true;
            Used = true;
            FillBrush = Brushes.DarkRed;
        }

        public void UpdateState()
        {
            IsAlive = _newState.IsAlive;
            FillBrush = _newState.FillBrush;
            Used = _newState.Used;
        }

        private Cell(bool t)
        {
            IsAlive = false;
            Used = false;
            FillBrush = Brushes.Transparent;
            _newState = null;
        }
        public void SetState(bool state)
        {
            if (state)
            {
                SetAlive();
            }
            else
            {
                SetDead();
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
