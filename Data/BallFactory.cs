namespace TPW.Data
{
    public class BallFactory : IBallFactory
    {
        public IBall Create(double x, double y, double radius, string color, double vx, double vy)
        {
            return new Ball(x, y, radius, color, vx, vy);
        }
    }
}