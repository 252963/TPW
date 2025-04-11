using TPW.Data;
using System;
using System.Collections.Generic;

namespace TPW.Logic
{
    public class BallLogic : IBallLogic
    {
        private readonly double _canvasWidth;
        private readonly double _canvasHeight;
        private readonly Random _rand = new();
        private readonly IBallFactory _ballFactory;

        public BallLogic(double canvasWidth, double canvasHeight, IBallFactory ballFactory)
        {
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;
            _ballFactory = ballFactory;
        }

        public void MoveBall(IBall ball)
        {
            var x = ball.X + ball.VX;
            var y = ball.Y + ball.VY;

            var vx = ball.VX;
            var vy = ball.VY;

            if (x - ball.Radius < 0 || x + ball.Radius > _canvasWidth)
                vx *= -1;

            if (y - ball.Radius < 0 || y + ball.Radius > _canvasHeight)
                vy *= -1;

            if (ball is Ball b)
            {
                b.X = x;
                b.Y = y;
                b.VX = vx;
                b.VY = vy;
            }
        }

        public List<IBall> CreateBalls(int count)
        {
            var balls = new List<IBall>();
            string[] colors = { "Red", "Green", "Blue", "Yellow", "Purple", "Orange" };

            for (int i = 0; i < count; i++)
            {
                balls.Add(_ballFactory.Create(
                    x: _rand.Next(20, (int)_canvasWidth - 20),
                    y: _rand.Next(20, (int)_canvasHeight - 20),
                    radius: _rand.Next(10, 20),
                    color: colors[_rand.Next(colors.Length)],
                    vx: _rand.NextDouble() * 6 - 3,
                    vy: _rand.NextDouble() * 6 - 3
                ));
            }

            return balls;
        }
    }
}