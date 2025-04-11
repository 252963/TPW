namespace TPW.Data
{
    public interface IBallFactory
    {
        IBall Create(double x, double y, double radius, string color, double vx, double vy);
    }
}
