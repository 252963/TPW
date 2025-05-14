using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPW.Data
{
    public class Ball : IBall
    {
        private readonly object _lock = new();
        private CancellationTokenSource? _cts;
        private Task? _movementTask;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Radius { get; }
        public string Color { get; }
        public double VX { get; private set; }
        public double VY { get; private set; }

        public double Mass => Math.PI * Radius * Radius;

        public event EventHandler? PositionChanged;

        public Ball(double x, double y, double radius, string color, double vx, double vy)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
            VX = vx;
            VY = vy;
        }

        public void Start()
        {
            if (_movementTask != null) return;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _movementTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    ShiftPosition(VX, VY);
                    await Task.Delay(16, token);
                }
            }, token);
        }

        public void Stop()
        {
            _cts?.Cancel();
            _movementTask = null;
        }

        public void ShiftPosition(double dx, double dy)
        {
            lock (_lock)
            {
                X += dx;
                Y += dy;
            }
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        public (double x, double y) GetPosition()
        {
            lock (_lock)
            {
                return (X, Y);
            }
        }

        public (double vx, double vy) GetVelocity()
        {
            lock (_lock)
            {
                return (VX, VY);
            }
        }

        public void SetVelocity(double vx, double vy)
        {
            lock (_lock)
            {
                VX = vx;
                VY = vy;
            }
        }

        public object GetLock()
        {
            return _lock;
        }
    }
}
