using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.ViewModel;

namespace TPW.Presentation.View
{
    public partial class MainWindow : Window
    {
        private BallViewModel _viewModel;
        private DispatcherTimer _timer;
        private IBallLogic _logic;

        private double CanvasWidth => BallCanvas.ActualWidth;
        private double CanvasHeight => BallCanvas.ActualHeight;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountTextBox.Text, out int count) && count > 0)
            {
                _logic = new BallLogic(new BallFactory());
                _logic.CreateBalls(count, CanvasWidth, CanvasHeight);
                _viewModel = new BallViewModel(_logic);

                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(30)
                };
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
            else
            {
                MessageBox.Show("Podaj poprawną liczbę kulek!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _viewModel.Update(0.03);
            RenderBalls();
        }

        private void RenderBalls()
        {
            BallCanvas.Children.Clear();

            foreach (var ball in _viewModel.Balls)
            {
                var ellipse = new Ellipse
                {
                    Width = ball.Radius * 2,
                    Height = ball.Radius * 2,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ball.Color))
                };
                Canvas.SetLeft(ellipse, ball.X - ball.Radius);
                Canvas.SetTop(ellipse, ball.Y - ball.Radius);
                BallCanvas.Children.Add(ellipse);
            }
        }
    }
}
