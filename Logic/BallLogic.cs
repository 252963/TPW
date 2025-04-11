using Data;
using System;
using System.Collections.Generic;

namespace TPW.Logic
{
    public class BallLogic
    {
        private readonly double _canvasWidth;
        private readonly double _canvasHeight;
        private readonly Random _rand = new();

        public BallLogic(double canvasWidth, double canvasHeight)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
        }

        public void MoveBall(Ball ball)
        {
            ball.X += ball.VX;
            ball.Y += ball.VY;

            if (ball.X - ball.Radius < 0 || ball.X + ball.Radius > _canvasWidth)
                ball.VX *= -1;

            if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > _canvasHeight)
                ball.VY *= -1;
        }

        public List<Ball> CreateBalls(int count)
        {
            var balls = new List<Ball>();
            string[] colors = { "Red", "Green", "Blue", "Yellow", "Purple", "Orange" };

            for (int i = 0; i < count; i++)
            {
                var ball = new Ball(
                    x: _rand.Next(20, (int)_canvasWidth - 20),
                    y: _rand.Next(20, (int)_canvasHeight - 20),
                    radius: _rand.Next(10, 20),
                    color: colors[_rand.Next(colors.Length)],
                    vx: _rand.NextDouble() * 6 - 3,
                    vy: _rand.NextDouble() * 6 - 3
                );

                balls.Add(ball);
            }

            return balls;
        }
    }
}
