using System.Windows;

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
