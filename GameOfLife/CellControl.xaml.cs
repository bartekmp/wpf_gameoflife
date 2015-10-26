using System;
using System.Collections.Generic;
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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        readonly Cell modelCell = new Cell();
        public CellControl()
        {
            InitializeComponent();
            DataContext = modelCell;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            modelCell.ChangeState();
            modelCell.ChangeCurrentState();
            // Btn.Background = modelCell.FillBrush;
        }

        public bool IsAlive
        {
            get { return modelCell.IsAlive; }
            set { modelCell.IsAlive = value; }
        }
        public bool Used
        {
            get { return modelCell.Used; }
            set { modelCell.Used = value; }
        } 

        public void ChangeState() { modelCell.ChangeState();}

        public void SetState(bool state) { modelCell.SetState(state);}

        public void SetDead() { modelCell.SetDead(); }
        public void SetAlive() { modelCell.SetAlive(); }
        public void UpdateState() { modelCell.UpdateState(); }
        public void SetCurrentDead() { modelCell.SetCurrentDead(); }
    }
}
