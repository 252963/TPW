using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.Model;
using TPW.Presentation.ViewModel;

namespace TPW.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly BallViewModel _viewModel;
        private readonly IBallLogic _logic;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            IBallFactory factory = new BallFactory();
            _logic = new BallLogic(factory);
            _logic.CreateBalls(10, 800, 600);

            _viewModel = new BallViewModel(_logic);
            DataContext = _viewModel;

            foreach (var ball in _logic.Balls)
            {
                ball.Start();
            }

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountBox.Text, out int count) && count > 0)
            {
                canvas.Children.Clear();
                _logic.CreateBalls(count, canvas.ActualWidth, canvas.ActualHeight);
                _logic.UpdateBounds(canvas.ActualWidth, canvas.ActualHeight);

                foreach (var ball in _logic.Balls)
                {
                    ball.Start();
                }

                _viewModel.RefreshBalls();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _logic.Update();
            RenderBalls();
        }

        private void RenderBalls()
        {
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
