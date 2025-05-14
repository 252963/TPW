using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.Model;
using TPW.Presentation.ViewModel;

namespace TPW.Presentation
{
    public partial class MainWindow : Window
    {
        private BallViewModel _viewModel;
        private IBallLogic _logic;

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

            CompositionTarget.Rendering += OnRendering;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountBox.Text, out int count) && count > 0)
            {
                canvas.Children.Clear();

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
