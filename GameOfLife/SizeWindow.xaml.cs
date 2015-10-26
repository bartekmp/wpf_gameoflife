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
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for SizeWindow.xaml
    /// </summary>
    public partial class SizeWindow
    {
        public int GameWidth { get; private set; }
        public int GameHeight { get; private set; }

        public SizeWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int width, height;
            width = int.TryParse(WidthBox.Text, out width) ? ((width >= 10 && width <= 100) ? width : 40) : 40;
            height = int.TryParse(HeightBox.Text, out height) ? ((height >= 10 && height <= 100) ? height : 20) : 20;
            GameWidth = width;
            GameHeight = height;
            Close();
        }
    }
}
