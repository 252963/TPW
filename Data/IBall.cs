using System;

namespace TPW.Data
{
    public interface IBall
    {
        double X { get; }
        double Y { get; }
        double Radius { get; }
        double VX { get; }
        double VY { get; }
        string Color { get; }
        double Mass { get; }

        void Start();
        void Stop();

        (double x, double y) GetPosition();
        (double vx, double vy) GetVelocity();
        void SetVelocity(double vx, double vy);
        void ShiftPosition(double dx, double dy);

        object GetLock();
        event EventHandler? PositionChanged;
    }
}
