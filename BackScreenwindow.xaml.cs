using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TradeClicker
{
    public partial class BackScreenWindow : Window
    {
        private Ellipse? crosshairA;
        private Ellipse? crosshairB;
        private double crosshairSize = 20;
        private Brush colorA = Brushes.Green;
        private Brush colorB = Brushes.Red;
        private double crosshairOpacity = 0.5;

        // Collections for Group A and Group B targets
        private readonly List<Ellipse> groupATargets = new List<Ellipse>();
        private readonly List<Ellipse> groupBTargets = new List<Ellipse>();

        public BackScreenWindow()
        {
            InitializeComponent();
            InitializeCrosshairs();
            InitializeTargets(); // Initialize targets similarly to the main window

            this.MouseMove += BackScreenWindow_MouseMove;
        }

        private void InitializeCrosshairs()
        {
            crosshairA = CreateCrosshair("Dot");
            crosshairB = CreateCrosshair("Dot");

            if (crosshairA != null)
            {
                Canvas.SetLeft(crosshairA, 0);
                Canvas.SetTop(crosshairA, 0);
                BackScreenCanvas.Children.Add(crosshairA);
            }

            if (crosshairB != null)
            {
                Canvas.SetLeft(crosshairB, 0);
                Canvas.SetTop(crosshairB, 0);
                BackScreenCanvas.Children.Add(crosshairB);
            }
        }

        private Ellipse CreateCrosshair(string type)
        {
            Ellipse crosshair = new Ellipse
            {
                Width = crosshairSize,
                Height = crosshairSize,
                Opacity = crosshairOpacity
            };

            switch (type)
            {
                case "Dot":
                    crosshair.Fill = Brushes.Black;
                    crosshair.Width = 5;
                    crosshair.Height = 5;
                    break;

                case "Circle":
                    crosshair.Fill = Brushes.Transparent;
                    crosshair.Stroke = Brushes.Black;
                    crosshair.StrokeThickness = 1;
                    crosshair.Width = crosshairSize;
                    crosshair.Height = crosshairSize;
                    break;

                case "Reticle":
                    crosshair.Fill = Brushes.Transparent;
                    crosshair.Stroke = Brushes.Black;
                    crosshair.StrokeThickness = 2;
                    break;

                default:
                    throw new ArgumentException("Unknown crosshair type");
            }

            return crosshair;
        }

        private void InitializeTargets()
        {
            BackScreenCanvas.Children.Clear();
            groupATargets.Clear();
            groupBTargets.Clear();

            for (int i = 0; i < 20; i++)
            {
                // Group A Targets
                Ellipse targetA = new Ellipse
                {
                    Width = crosshairSize,
                    Height = crosshairSize,
                    Fill = colorA
                };
                Canvas.SetLeft(targetA, i * (crosshairSize + 10) + 10);
                Canvas.SetTop(targetA, 50);
                BackScreenCanvas.Children.Add(targetA);
                groupATargets.Add(targetA);

                // Group B Targets
                Ellipse targetB = new Ellipse
                {
                    Width = crosshairSize,
                    Height = crosshairSize,
                    Fill = colorB
                };
                Canvas.SetLeft(targetB, i * (crosshairSize + 10) + 10);
                Canvas.SetTop(targetB, 150);
                BackScreenCanvas.Children.Add(targetB);
                groupBTargets.Add(targetB);
            }
        }

        private void BackScreenWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(BackScreenCanvas);

            // Update crosshair positions
            if (crosshairA != null)
            {
                Canvas.SetLeft(crosshairA, mousePosition.X - crosshairA.Width / 2);
                Canvas.SetTop(crosshairA, mousePosition.Y - crosshairA.Height / 2);
            }

            if (crosshairB != null)
            {
                Canvas.SetLeft(crosshairB, mousePosition.X - crosshairB.Width / 2);
                Canvas.SetTop(crosshairB, mousePosition.Y - crosshairB.Height / 2);
            }
        }

        // Public method to simulate a click from the main window
        public void SimulateClick(Point clickPosition)
        {
            // Convert clickPosition to screen coordinates
            Point screenPoint = this.PointToScreen(clickPosition);

            // Move crosshairs to the clicked position
            if (crosshairA != null)
            {
                Canvas.SetLeft(crosshairA, screenPoint.X - crosshairA.Width / 2);
                Canvas.SetTop(crosshairA, screenPoint.Y - crosshairA.Height / 2);
            }

            if (crosshairB != null)
            {
                Canvas.SetLeft(crosshairB, screenPoint.X - crosshairB.Width / 2);
                Canvas.SetTop(crosshairB, screenPoint.Y - crosshairB.Height / 2);
            }
        }
    }
}
