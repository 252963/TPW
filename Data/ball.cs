using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPW.Data
{
    public class Ball : IBall
    {
        private readonly object _lock = new();
        private readonly DiagnosticLogger? _logger;
        private CancellationTokenSource? _cts;
        private Task? _movementTask;

        private double _x, _y;

        public double X { get { lock (_lock) { return _x; } } }
        public double Y { get { lock (_lock) { return _y; } } }
        public double Radius { get; }
        public string Color { get; }
        public double VX { get; private set; }
        public double VY { get; private set; }

        public double Mass => Math.PI * Radius * Radius;

        public event EventHandler? PositionChanged;

        public Ball(double x, double y, double radius, string color, double vx, double vy, DiagnosticLogger? logger = null)
        {
            _x = x;
            _y = y;
            Radius = radius;
            Color = color;
            VX = vx;
            VY = vy;
            _logger = logger;
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
                    await Task.Delay(16, token); // 60 FPS
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
                _x += dx;
                _y += dy;
                _logger?.Log($"Ball moved to ({_x:F2}, {_y:F2})");
            }
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        public (double x, double y) GetPosition()
        {
            lock (_lock)
            {
                return (_x, _y);
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
                _logger?.Log($"Velocity set to ({VX:F2}, {VY:F2})");
            }
        }

        public object GetLock()
        {
            return _lock;
        }
    }
}
