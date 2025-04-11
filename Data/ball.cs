namespace TPW.Data
{
    public class Ball
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }
        public string Color { get; set; }
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
}