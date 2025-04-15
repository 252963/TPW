using System.Collections.Generic;
using TPW.Data;
using TPW.Logic;
using Xunit;

namespace UnitsTests
{
    public class FakeBall : IBall
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; } = 10;
        public double VX { get; set; }
        public double VY { get; set; }
        public string Color { get; set; } = "Red";
    }

    public class FakeBallFactory : IBallFactory
    {
        public IBall Create(double x, double y, double radius, string color, double vx, double vy)
            => new FakeBall { X = x, Y = y, Radius = radius, Color = color, VX = vx, VY = vy };
    }

    public class BallLogicTests
    {
        [Fact]
        public void MoveBall_ShouldBounceHorizontally_WhenHitsLeftWall()
        {
            var ball = new FakeBall { X = 5, Y = 50, VX = -3, VY = 0, Radius = 10 };
            var logic = new BallLogic(100, 100, new FakeBallFactory());

            logic.MoveBall(ball);

            Assert.True(ball.VX > 0);
        }

        [Fact]
        public void MoveBall_ShouldBounceVertically_WhenHitsTopWall()
        {
            var ball = new FakeBall { X = 50, Y = 5, VX = 0, VY = -2, Radius = 10 };
            var logic = new BallLogic(100, 100, new FakeBallFactory());

            logic.MoveBall(ball);

            Assert.True(ball.VY > 0);
        }

        [Fact]
        public void CreateBalls_ShouldReturnCorrectNumberOfBalls()
        {
            var logic = new BallLogic(200, 200, new FakeBallFactory());

            List<IBall> balls = logic.CreateBalls(5);

            Assert.Equal(5, balls.Count);
        }
    }
}
