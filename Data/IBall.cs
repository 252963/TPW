namespace TPW.Data
{
    public interface IBall
    {
        double X { get; set; }
        double Y { get; set; }
        double Radius { get; }
        string Color { get; }
        double VX { get; set; }
        double VY { get; set; }
    }
}
