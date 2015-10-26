using System.Windows;
using System.Windows.Controls;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        readonly Cell _modelCell = new Cell();
        public CellControl()
        {
            InitializeComponent();
            DataContext = _modelCell;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _modelCell.ChangeState();
        }

        public bool IsAlive
        {
            get { return _modelCell.IsAlive; }
            set { _modelCell.IsAlive = value; }
        }
        public bool Used
        {
            get { return _modelCell.Used; }
            set { _modelCell.Used = value; }
        } 

        public void ChangeState() { _modelCell.ChangeState();}

        public void SetState(bool state) { _modelCell.SetState(state);}

        public void SetDead() { _modelCell.SetDead(); }
        public void SetAlive() { _modelCell.SetAlive(); }
    }
}
