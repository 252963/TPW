using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.Model;
using TPW.Presentation.ViewModel;
using System.IO;

namespace TPW.Presentation
{
    public partial class MainWindow : Window
    {
        private BallViewModel _viewModel;
        private IBallLogic _logic;

        public MainWindow()
        {
            InitializeComponent();

            string logDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logPath = System.IO.Path.Combine(logDirectory, $"log_{timestamp}.txt");

            var logger = new DiagnosticLogger(logPath);
            IBallFactory factory = new BallFactory(logger);
            _logic = new BallLogic(factory, logger);
            _logic.CreateBalls(10, 800, 600);

            _viewModel = new BallViewModel(_logic);
            DataContext = _viewModel;

            foreach (var ball in _logic.Balls)
            {
                ball.Start();
            }

            CompositionTarget.Rendering += OnRendering;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountBox.Text, out int count) && count > 0)
            {
                canvas.Children.Clear();

                string logDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logDirectory);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string logPath = System.IO.Path.Combine(logDirectory, $"log_{timestamp}.txt");

                var logger = new DiagnosticLogger(logPath);
                IBallFactory factory = new BallFactory(logger);
                _logic = new BallLogic(factory, logger);
                _logic.CreateBalls(count, canvas.ActualWidth, canvas.ActualHeight);
                _logic.UpdateBounds(canvas.ActualWidth, canvas.ActualHeight);

                _viewModel = new BallViewModel(_logic);
                DataContext = _viewModel;

                foreach (var ball in _logic.Balls)
                {
                    ball.Start();
                }
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _logic.Update();
            RenderBalls();
        }

        private void RenderBalls()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(RenderBalls);
                return;
            }

            canvas.Children.Clear();

            foreach (BallModel ball in _viewModel.Balls)
            {
                var ellipse = new Ellipse
                {
                    Width = ball.Radius * 2,
                    Height = ball.Radius * 2,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ball.Color))
                };

                Canvas.SetLeft(ellipse, ball.X - ball.Radius);
                Canvas.SetTop(ellipse, ball.Y - ball.Radius);
                canvas.Children.Add(ellipse);
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_logic != null && canvas != null)
            {
                _logic.UpdateBounds(canvas.ActualWidth, canvas.ActualHeight);
            }
        }
    }
}
