using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ElectronNET.API;

namespace TradeClicker
{
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
    }

    public partial class MainWindow : Window
    {
        private Ellipse crosshairA;
        private Ellipse crosshairB;
        private double crosshairSize = 20;
        private Brush colorA = Brushes.Green;
        private Brush colorB = Brushes.Red;
        private double crosshairOpacity = 0.5;

        private readonly List<Ellipse> groupATargets = new List<Ellipse>();
        private readonly List<Ellipse> groupBTargets = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeCrosshairs();
            InitializeTargets();

            SizeSlider.ValueChanged += SizeSlider_ValueChanged;
            OpacitySlider.ValueChanged += OpacitySlider_ValueChanged;
            ColorAButton.Click += ColorAButton_Click;
            ColorBButton.Click += ColorBButton_Click;
            CrosshairCanvas.MouseMove += CrosshairCanvas_MouseMove;

            DotCrosshairButton.Click += DotCrosshairButton_Click;
            CircleCrosshairButton.Click += CircleCrosshairButton_Click;
            ReticleCrosshairButton.Click += ReticleCrosshairButton_Click;

            SingleTargetModeButton.Click += SingleTargetMode_Click;
            MultiTargetModeButton.Click += MultiTargetMode_Click;
            OpenSettingsButton.Click += OpenSettings_Click;
            RemoveAdsButton.Click += RemoveAds_Click; // Fixed assignment

            InitializeElectron();
        }

        private async void InitializeElectron()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.App.Ready += async () =>
                {
                    var mainWindow = await Electron.WindowManager.CreateWindowAsync();
                    mainWindow.OnClosed += () => Electron.App.Quit();
                };
            }
        }

        private void InitializeCrosshairs()
        {
            crosshairA = CreateCrosshair("Dot");
            crosshairB = CreateCrosshair("Dot");

            Canvas.SetLeft(crosshairA, 0);
            Canvas.SetTop(crosshairA, 0);
            CrosshairCanvas.Children.Add(crosshairA);

            Canvas.SetLeft(crosshairB, 0);
            Canvas.SetTop(crosshairB, 0);
            CrosshairCanvas.Children.Add(crosshairB);
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
                    crosshair.Width = crosshairSize;
                    crosshair.Height = crosshairSize;
                    crosshair.Fill = Brushes.Transparent;
                    crosshair.Stroke = Brushes.Black;
                    crosshair.StrokeThickness = 2;
                    break;

                default:
                    throw new ArgumentException("Unknown crosshair type");
            }

            return crosshair;
        }

        private void ChangeCrosshairType(string type)
        {
            crosshairA = CreateCrosshair(type);
            crosshairB = CreateCrosshair(type);

            CrosshairCanvas.Children.Clear();

            Canvas.SetLeft(crosshairA, 0);
            Canvas.SetTop(crosshairA, 0);
            CrosshairCanvas.Children.Add(crosshairA);

            Canvas.SetLeft(crosshairB, 0);
            Canvas.SetTop(crosshairB, 0);
            CrosshairCanvas.Children.Add(crosshairB);
        }

        private void DotCrosshairButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeCrosshairType("Dot");
        }

        private void CircleCrosshairButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeCrosshairType("Circle");
        }

        private void ReticleCrosshairButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeCrosshairType("Reticle");
        }

        private void InitializeTargets()
        {
            CrosshairCanvas.Children.Clear();
            groupATargets.Clear();
            groupBTargets.Clear();

            for (int i = 0; i < 20; i++)
            {
                Ellipse targetA = new Ellipse
                {
                    Width = crosshairSize,
                    Height = crosshairSize,
                    Fill = colorA
                };
                Canvas.SetLeft(targetA, i * (crosshairSize + 10) + 10);
                Canvas.SetTop(targetA, 50);
                CrosshairCanvas.Children.Add(targetA);
                groupATargets.Add(targetA);

                Ellipse targetB = new Ellipse
                {
                    Width = crosshairSize,
                    Height = crosshairSize,
                    Fill = colorB
                };
                Canvas.SetLeft(targetB, i * (crosshairSize + 10) + 10);
                Canvas.SetTop(targetB, 100);
                CrosshairCanvas.Children.Add(targetB);
                groupBTargets.Add(targetB);
            }
        }

        private void UpdateCrosshairSize(double size)
        {
            crosshairSize = size;
            crosshairA.Width = size;
            crosshairA.Height = size;
            crosshairB.Width = size;
            crosshairB.Height = size;

            foreach (var target in groupATargets)
            {
                target.Width = size;
                target.Height = size;
            }

            foreach (var target in groupBTargets)
            {
                target.Width = size;
                target.Height = size;
            }
        }

        private void UpdateCrosshairOpacity(double opacity)
        {
            crosshairOpacity = opacity;
            crosshairA.Opacity = opacity;
            crosshairB.Opacity = opacity;
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCrosshairSize(e.NewValue);
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateCrosshairOpacity(e.NewValue);
        }

        private void ColorAButton_Click(object sender, RoutedEventArgs e)
        {
            // Open color picker for Color A
        }

        private void ColorBButton_Click(object sender, RoutedEventArgs e)
        {
            // Open color picker for Color B
        }

        private void CrosshairCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(CrosshairCanvas);
            Canvas.SetLeft(crosshairA, position.X - crosshairSize / 2);
            Canvas.SetTop(crosshairA, position.Y - crosshairSize / 2);
        }

        private void SingleTargetMode_Click(object sender, RoutedEventArgs e)
        {
            // Start single target mode
        }

        private void MultiTargetMode_Click(object sender, RoutedEventArgs e)
        {
            // Start multi target mode
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            // Open settings
        }

        private void RemoveAds_Click(object sender, RoutedEventArgs e)
        {
            // Remove ads functionality
        }
    }
}
