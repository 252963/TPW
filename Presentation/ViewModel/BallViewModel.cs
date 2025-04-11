using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Timers;
using System.Windows;
using TPW.Logic;
using TPW.Data;
using Timer = System.Timers.Timer;
using System.Windows.Controls;

namespace TPW.Presentation.ViewModel
{
    public class BallViewModel : IDisposable
    {
        public ObservableCollection<Ellipse> Balls { get; private set; } = new();

        private readonly List<IBall> _ballModels = new();
        private readonly IBallLogic _ballLogic;
        private readonly Timer _timer;
        private readonly double _canvasWidth = 400;
        private readonly double _canvasHeight = 400;

        public BallViewModel(int ballCount, IBallLogic ballLogic)
        {
            _ballLogic = ballLogic;

            var balls = _ballLogic.CreateBalls(ballCount);
            _ballModels.AddRange(balls);

            foreach (var ball in balls)
            {
                var ellipse = new Ellipse
                {
                    Width = ball.Radius * 2,
                    Height = ball.Radius * 2,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ball.Color))
                };

                Balls.Add(ellipse);
            }

            _timer = new Timer(30);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (Application.Current == null || Application.Current.Dispatcher.HasShutdownStarted)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < _ballModels.Count; i++)
                {
                    var ball = _ballModels[i];
                    _ballLogic.MoveBall(ball);

                    Canvas.SetLeft(Balls[i], ball.X - ball.Radius);
                    Canvas.SetTop(Balls[i], ball.Y - ball.Radius);
                }
            });
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}