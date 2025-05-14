using System;

namespace TPW.Data
{
    public interface IBall
    {
        double X { get; }
        double Y { get; }
        double Radius { get; }
        string Color { get; }
        double VX { get; }
        double VY { get; }

        void Start();
        void Stop();

        (double x, double y) GetPosition();
        (double vx, double vy) GetVelocity();
        void SetVelocity(double vx, double vy);
        void ShiftPosition(double dx, double dy);

        event EventHandler? PositionChanged;

        object GetLock();
    }
}
