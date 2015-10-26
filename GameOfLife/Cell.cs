using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
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
                OnPropertyChanged("FillBrush");
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
            _newState.FillBrush = Brushes.Black;
        }

        public void SetDead()
        {
            _newState.IsAlive = false;
            _newState.FillBrush = Used ? Brushes.CornflowerBlue : Brushes.Transparent;
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
                FillBrush = Used ? Brushes.CornflowerBlue : Brushes.Transparent;
            }
            else
            {
                IsAlive = true;
                Used = true;
                FillBrush = Brushes.Black;
            }
        }

        public void SetCurrentDead()
        {
            IsAlive = false;
            Used = false;
            FillBrush = Used ? Brushes.CornflowerBlue : Brushes.Transparent;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
