using System;
using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic
{
    public class BallLogic : IBallLogic
    {
        private readonly List<IBall> _balls = new();
        private readonly IBallFactory _factory;
        private readonly DiagnosticLogger _logger;
        private double _width;
        private double _height;

        public BallLogic(IBallFactory factory, DiagnosticLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public IEnumerable<IBall> Balls => _balls;

        public void CreateBalls(int count, double width, double height)
        {
            _balls.Clear();
            _width = width;
            _height = height;
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                double radius = 10 + random.NextDouble() * 20;
                var ball = _factory.Create(
                    random.NextDouble() * (width - 2 * radius) + radius,
                    random.NextDouble() * (height - 2 * radius) + radius,
                    radius,
                    "Red",
                    (random.NextDouble() * 200 - 100) * 0.02,
                    (random.NextDouble() * 200 - 100) * 0.02
                );

                _balls.Add(ball);
            }
        }

        public void Update()
        {
            Update(0.016);
        }

        public void Update(double deltaTime)
        {
            HandleWallCollisions();
            HandleBallCollisions();
        }

        public void UpdateBounds(double width, double height)
        {
            _width = width;
            _height = height;
        }

        private void HandleWallCollisions()
        {
            foreach (var ball in _balls)
            {
                lock (ball.GetLock())
                {
                    var (x, y) = ball.GetPosition();
                    var (vx, vy) = ball.GetVelocity();
                    double r = ball.Radius;

                    if (x - r < 0)
                    {
                        x = r;
                        vx = -vx;
                    }
                    else if (x + r > _width)
                    {
                        x = _width - r;
                        vx = -vx;
                    }

                    if (y - r < 0)
                    {
                        y = r;
                        vy = -vy;
                    }
                    else if (y + r > _height)
                    {
                        y = _height - r;
                        vy = -vy;
                    }

                    ball.SetVelocity(vx, vy);
                    ball.ShiftPosition(x - ball.X, y - ball.Y);
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

                    lock (b1.GetLock())
                        lock (b2.GetLock())
                        {
                            double dx = b2.X - b1.X;
                            double dy = b2.Y - b1.Y;
                            double distance = Math.Sqrt(dx * dx + dy * dy);
                            double minDist = b1.Radius + b2.Radius;

                            if (distance < minDist && distance > 0.0001)
                            {
                                // Środek kolizji
                                double collisionX = (b1.X + b2.X) / 2;
                                double collisionY = (b1.Y + b2.Y) / 2;
                                _logger.Log($"Collision detected between ball {i} and ball {j} at point ({collisionX:F2}, {collisionY:F2})");

                                double overlap = 0.5 * (minDist - distance);
                                double ox = overlap * dx / distance;
                                double oy = overlap * dy / distance;

                                b1.ShiftPosition(-ox, -oy);
                                b2.ShiftPosition(ox, oy);

                                var (v1x, v1y) = b1.GetVelocity();
                                var (v2x, v2y) = b2.GetVelocity();

                                double m1 = b1.Mass;
                                double m2 = b2.Mass;

                                double nx = dx / distance;
                                double ny = dy / distance;

                                double p = 2 * (v1x * nx + v1y * ny - v2x * nx - v2y * ny) / (m1 + m2);

                                v1x -= p * m2 * nx;
                                v1y -= p * m2 * ny;
                                v2x += p * m1 * nx;
                                v2y += p * m1 * ny;

                                b1.SetVelocity(v1x, v1y);
                                b2.SetVelocity(v2x, v2y);
                            }
                        }
                }
            }
        }
    }
}
