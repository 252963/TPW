namespace TPW.Data
{
    public class Ball : IBall
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; private set; }
        public string Color { get; private set; }
        public double VX { get; set; }
        public double VY { get; set; }

        public Ball(double x, double y, double radius, string color, double vx, double vy)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
            VX = vx;
            VY = vy;
        }
    }

    public class BallFactory : IBallFactory
    {
        public IBall Create(double x, double y, double radius, string color, double vx, double vy)
        {
            return new Ball(x, y, radius, color, vx, vy);
        }
    }
}