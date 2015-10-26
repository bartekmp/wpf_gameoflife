using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class Cell : INotifyPropertyChanged
    {
        public bool IsAlive { get; set; }
        public bool Used { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
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
        public Cell(int x=0, int y=0)
        {
            X = x;
            Y = y;
            IsAlive = false;
            Used = false;
            FillBrush = Brushes.Transparent;


        }

        public void SetAlive()
        {
            IsAlive = true;
            Used = true;
            FillBrush = Brushes.Black;
        }

        public void SetDead()
        {
            IsAlive = false;
            FillBrush = Used ? Brushes.CornflowerBlue : Brushes.Transparent;
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
