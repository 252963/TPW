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
            _balls.Clear();
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

                    // Unikamy deadlocków: zawsze b1, potem b2
                    var locks = new[] { b1, b2 };
                    Array.Sort(locks, (a, b) => a.GetHashCode().CompareTo(b.GetHashCode()));

                    lock (locks[0].GetLock())
                        lock (locks[1].GetLock())
                        {
                            var (x1, y1) = b1.GetPosition();
                            var (x2, y2) = b2.GetPosition();

                            double dx = x2 - x1;
                            double dy = y2 - y1;
                            double distance = Math.Sqrt(dx * dx + dy * dy);
                            double minDist = b1.Radius + b2.Radius;

                            if (distance < minDist && distance > 0)
                            {
                                var (vx1, vy1) = b1.GetVelocity();
                                var (vx2, vy2) = b2.GetVelocity();

                                double angle = Math.Atan2(dy, dx);
                                double speed1 = Math.Sqrt(vx1 * vx1 + vy1 * vy1);
                                double speed2 = Math.Sqrt(vx2 * vx2 + vy2 * vy2);
                                double direction1 = Math.Atan2(vy1, vx1);
                                double direction2 = Math.Atan2(vy2, vx2);

                                double newVx1 = speed1 * Math.Cos(direction1 - angle);
                                double newVy1 = speed1 * Math.Sin(direction1 - angle);
                                double newVx2 = speed2 * Math.Cos(direction2 - angle);
                                double newVy2 = speed2 * Math.Sin(direction2 - angle);

                                double finalVx1 = newVx2;
                                double finalVx2 = newVx1;

                                double vx1Final = Math.Cos(angle) * finalVx1 + Math.Cos(angle + Math.PI / 2) * newVy1;
                                double vy1Final = Math.Sin(angle) * finalVx1 + Math.Sin(angle + Math.PI / 2) * newVy1;
                                double vx2Final = Math.Cos(angle) * finalVx2 + Math.Cos(angle + Math.PI / 2) * newVy2;
                                double vy2Final = Math.Sin(angle) * finalVx2 + Math.Sin(angle + Math.PI / 2) * newVy2;

                                b1.SetVelocity(vx1Final, vy1Final);
                                b2.SetVelocity(vx2Final, vy2Final);

                                double overlap = 0.5 * (minDist - distance);
                                double offsetX = overlap * (dx / distance);
                                double offsetY = overlap * (dy / distance);

                                b1.ShiftPosition(-offsetX, -offsetY);
                                b2.ShiftPosition(offsetX, offsetY);
                            }
                        }
                }
            }
        }
    }
}
