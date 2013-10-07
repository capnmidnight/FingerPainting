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
        int? currentID;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTouch(int id, Point p)
        {
            if (!ellipses.ContainsKey(id))
            {
                var ellipse = new Ellipse();
                ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
                ellipse.Stroke = new SolidColorBrush(Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256)));
                ellipse.StrokeThickness = 5;
                ellipse.Width = 10;
                ellipse.Height = 10;
                canv.Children.Add(ellipse);
                Canvas.SetLeft(ellipse, p.X - 5);
                Canvas.SetTop(ellipse, p.Y - 5);
                ellipses.Add(id, ellipse);
            }
        }

        private void MoveTouch(int id, Point p)
        {
            if (ellipses.ContainsKey(id))
            {
                var ellipse = ellipses[id];

                var x = Canvas.GetLeft(ellipse) + ellipse.Width / 2;
                var y = Canvas.GetTop(ellipse) + ellipse.Height / 2;
                var dx = Math.Abs(p.X - x);
                var dy = Math.Abs(p.Y - y);
                Canvas.SetLeft(ellipse, x - dx);
                Canvas.SetTop(ellipse, y - dy);
                ellipse.Width = dx * 2;
                ellipse.Height = dy * 2;
            }
        }

        private void RemoveTouch(int id, Point p)
        {
            if (ellipses.ContainsKey(id))
                ellipses.Remove(id);
        }

        private void canv_TouchDown(object sender, TouchEventArgs e)
        {
            var p = e.GetTouchPoint(canv);
            AddTouch(p.TouchDevice.Id, p.Position);
        }

        private void canv_TouchMove(object sender, TouchEventArgs e)
        {
            var p = e.GetTouchPoint(canv);
            MoveTouch(p.TouchDevice.Id, p.Position);
        }

        private void canv_TouchUp(object sender, TouchEventArgs e)
        {
            var p = e.GetTouchPoint(canv);
            RemoveTouch(p.TouchDevice.Id, p.Position);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int id = ellipses.Count == 0 
                ? 1
                : ellipses.Keys.Max() + 1;

            AddTouch(id, e.GetPosition(canv));

            currentID = id;
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentID.HasValue)
                MoveTouch(currentID.Value, e.GetPosition(canv));
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(currentID.HasValue)
            {
                RemoveTouch(currentID.Value, e.GetPosition(canv));
                currentID = null;
            }
        }
    }
}
