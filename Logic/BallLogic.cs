using System;
using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic
{
    public class BallLogic : IBallLogic
    {
        private readonly List<IBall> _balls = new();
        private readonly IBallFactory _factory;
        private double _width;
        private double _height;

        public BallLogic(IBallFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<IBall> Balls => _balls;

        public void CreateBalls(int count, double width, double height)
        {
            _width = width;
            _height = height;
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                double radius = 10;
                var ball = _factory.Create(
                    random.NextDouble() * (width - 2 * radius) + radius,
                    random.NextDouble() * (height - 2 * radius) + radius,
                    radius,
                    "Red",
                    random.NextDouble() * 200 - 100,
                    random.NextDouble() * 200 - 100
                );
                _balls.Add(ball);
            }
        }

        public void Update(double deltaTime)
        {
            foreach (var ball in _balls)
            {
                ball.X += ball.VX * deltaTime;
                ball.Y += ball.VY * deltaTime;
            }

            HandleWallCollisions();
            HandleBallCollisions();
        }

        private void HandleWallCollisions()
        {
            foreach (var ball in _balls)
            {
                if (ball.X - ball.Radius < 0)
                {
                    ball.X = ball.Radius;
                    ball.VX *= -1;
                }
                else if (ball.X + ball.Radius > _width)
                {
                    ball.X = _width - ball.Radius;
                    ball.VX *= -1;
                }

                if (ball.Y - ball.Radius < 0)
                {
                    ball.Y = ball.Radius;
                    ball.VY *= -1;
                }
                else if (ball.Y + ball.Radius > _height)
                {
                    ball.Y = _height - ball.Radius;
                    ball.VY *= -1;
                }
            }
        }

        private void HandleBallCollisions()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                for (int j = i + 1; j < _balls.Count; j++)
                {
                    var b1 = _balls[i];
                    var b2 = _balls[j];
                    double dx = b2.X - b1.X;
                    double dy = b2.Y - b1.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    if (distance < b1.Radius + b2.Radius && distance > 0)
                    {
                        double angle = Math.Atan2(dy, dx);

                        double speed1 = Math.Sqrt(b1.VX * b1.VX + b1.VY * b1.VY);
                        double speed2 = Math.Sqrt(b2.VX * b2.VX + b2.VY * b2.VY);

                        double direction1 = Math.Atan2(b1.VY, b1.VX);
                        double direction2 = Math.Atan2(b2.VY, b2.VX);

                        double newVx1 = speed1 * Math.Cos(direction1 - angle);
                        double newVy1 = speed1 * Math.Sin(direction1 - angle);
                        double newVx2 = speed2 * Math.Cos(direction2 - angle);
                        double newVy2 = speed2 * Math.Sin(direction2 - angle);

                        double finalVx1 = newVx2;
                        double finalVx2 = newVx1;

                        b1.VX = Math.Cos(angle) * finalVx1 + Math.Cos(angle + Math.PI / 2) * newVy1;
                        b1.VY = Math.Sin(angle) * finalVx1 + Math.Sin(angle + Math.PI / 2) * newVy1;
                        b2.VX = Math.Cos(angle) * finalVx2 + Math.Cos(angle + Math.PI / 2) * newVy2;
                        b2.VY = Math.Sin(angle) * finalVx2 + Math.Sin(angle + Math.PI / 2) * newVy2;

                        // 🔥 Naprawa: rozdzielenie pozycji po kolizji
                        double overlap = 0.5 * (b1.Radius + b2.Radius - distance);
                        double offsetX = overlap * (dx / distance);
                        double offsetY = overlap * (dy / distance);

                        b1.X -= offsetX;
                        b1.Y -= offsetY;
                        b2.X += offsetX;
                        b2.Y += offsetY;
                    }
                }
            }
        }

    }
}
