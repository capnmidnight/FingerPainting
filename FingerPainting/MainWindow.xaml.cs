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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        Dictionary<int, Ellipse> ellipses = new Dictionary<int, Ellipse>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTouch(TouchPoint p)
        {
            if (!ellipses.ContainsKey(p.TouchDevice.Id))
            {
                var ellipse = new Ellipse();
                ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
                ellipse.Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
                ellipse.StrokeThickness = 5;
                ellipse.Width = 10;
                ellipse.Height = 10;
                canv.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, p.Position.X - 5);
                Canvas.SetTop(ellipse, p.Position.Y - 5);
                ellipses.Add(p.TouchDevice.Id, ellipse);
            }
        }

        private void MoveTouch(TouchPoint p)
        {
            if (ellipses.ContainsKey(p.TouchDevice.Id))
            {
                var ellipse = ellipses[p.TouchDevice.Id];

                var x = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
                var y = Canvas.GetTop(ellipse) + ellipse.Height / 2;
                var dx = Math.Abs(p.Position.X - x);
                var dy = Math.Abs(p.Position.Y - y);
                Canvas.SetLeft(ellipse, x - dx);
                Canvas.SetTop(ellipse, y - dy);
                ellipse.Width = dx * 2;
                ellipse.Height = dy * 2;
            }
        }

        private void RemoveTouch(TouchPoint p)
        {
            if (ellipses.ContainsKey(p.TouchDevice.Id))
            {
                ellipses.Remove(p.TouchDevice.Id);
            }
        }

        private void canv_TouchDown(object sender, TouchEventArgs e)
        {
            AddTouch(e.GetTouchPoint(canv));
        }

        private void canv_TouchMove(object sender, TouchEventArgs e)
        {
            MoveTouch(e.GetTouchPoint(canv));
        }

        private void canv_TouchUp(object sender, TouchEventArgs e)
        {
            RemoveTouch(e.GetTouchPoint(canv));
        }
    }
}
