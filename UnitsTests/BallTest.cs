using TPW.Data;
using Xunit;

namespace UnitsTests
{
    public class BallTest
    {
        [Fact]
        public void Constructor_SetsAllFieldsCorrectly()
        {
            var ball = new Ball(1, 2, 10, "Blue", 1.5, -2.5);

            Assert.Equal(1, ball.X);
            Assert.Equal(2, ball.Y);
            Assert.Equal(10, ball.Radius);
            Assert.Equal("Blue", ball.Color);
            Assert.Equal(1.5, ball.VX);
            Assert.Equal(-2.5, ball.VY);
        }
    }
}
