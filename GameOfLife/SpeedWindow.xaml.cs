using System.Windows;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for SpeedWindow.xaml
    /// </summary>
    public partial class SpeedWindow
    {
        public SpeedWindow()
        {
            InitializeComponent();
        }

        public double SliderValue { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SliderValue = Slider.Value;
            Close();

        }
    }
}
