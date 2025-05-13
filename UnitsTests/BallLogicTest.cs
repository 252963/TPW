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

        public void Update()
        {
            HandleWallCollisions();
            HandleBallCollisions();
        }

        private void HandleWallCollisions()
        {
            foreach (var ball in _balls)
            {
                var (x, y) = ball.GetPosition();

                if (x - ball.Radius <= 0 || x + ball.Radius >= _width)
                {
                    ball.SetVelocity(-ball.VX, ball.VY);
                }

                if (y - ball.Radius <= 0 || y + ball.Radius >= _height)
                {
                    ball.SetVelocity(ball.VX, -ball.VY);
                }
            }
        }

        private void HandleBallCollisions()
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                for (int j = i + 1; j < _balls.Count; j++)
                {
                    var ballA = _balls[i];
                    var ballB = _balls[j];

                    var (ax, ay) = ballA.GetPosition();
                    var (bx, by) = ballB.GetPosition();

                    double dx = bx - ax;
                    double dy = by - ay;
                    double distanceSquared = dx * dx + dy * dy;
                    double minDistance = ballA.Radius + ballB.Radius;

                    if (distanceSquared < minDistance * minDistance)
                    {
                        double distance = Math.Sqrt(distanceSquared);
                        if (distance == 0) continue;

                        double nx = dx / distance;
                        double ny = dy / distance;

                        double v1n = ballA.VX * nx + ballA.VY * ny;
                        double v2n = ballB.VX * nx + ballB.VY * ny;

                        double v1t = -ny * ballA.VX + nx * ballA.VY;
                        double v2t = -ny * ballB.VX + nx * ballB.VY;

                        double newV1n = v2n;
                        double newV2n = v1n;

                        double v1x = newV1n * nx - v1t * ny;
                        double v1y = newV1n * ny + v1t * nx;
                        double v2x = newV2n * nx - v2t * ny;
                        double v2y = newV2n * ny + v2t * nx;

                        ballA.SetVelocity(v1x, v1y);
                        ballB.SetVelocity(v2x, v2y);
                    }
                }
            }
        }
    }
}
