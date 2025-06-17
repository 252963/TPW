using System;
using System.IO;

namespace TPW.Data
{
    public class BallFactory : IBallFactory
    {
        private readonly DiagnosticLogger _logger;

        public BallFactory(DiagnosticLogger logger)
        {
            _logger = logger;
        }

        public IBall Create(double x, double y, double radius, string color, double vx, double vy)
        {
            var ball = new Ball(x, y, radius, color, vx, vy, _logger);
            ball.Start(); // uruchomienie taska po utworzeniu piłki
            return ball;
        }
    }
}
