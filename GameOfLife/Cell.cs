using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    public class Cell
    {
        public Rectangle Rect;
        public Button Btn;
        public bool IsAlive { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            IsAlive = false;

            Rect = new Rectangle
            {
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black,
            };
            Btn = new Button
            {
                Background = Brushes.Transparent
            };

        }

        public void SetAlive()
        {
            IsAlive = true;
            Rect.Fill = Brushes.Black;
        }

        public void SetDead()
        {
            IsAlive = false;
            Rect.Fill = Brushes.Transparent;
        }

        public void ChangeState()
        {
            IsAlive = !IsAlive;
            Rect.Fill = IsAlive ? Brushes.Black : Brushes.Transparent;
        }

        public void SetState(bool state)
        {
            IsAlive = state;
            Rect.Fill = IsAlive ? Brushes.Black : Brushes.Transparent;
        }

   

    }
}
