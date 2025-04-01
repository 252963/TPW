namespace TPW.Data
{
    public class Ball
    {
        public double X { get; }
        public double Y { get; }
        public double Radius { get; }
        public string Color { get; }

        public Ball(double x, double y, double radius = 10, string color = "Blue")
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
        }
    }
}
