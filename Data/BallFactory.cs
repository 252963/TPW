using System;
using System.IO;

namespace TPW.Data
{
    public class BallFactory : IBallFactory
    {
        private static readonly DiagnosticLogger logger = new DiagnosticLogger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "diagnostic_log.txt"));

        public IBall Create(double x, double y, double radius, string color, double vx, double vy)
        {
            var ball = new Ball(x, y, radius, color, vx, vy, logger);
            ball.Start(); // uruchomienie taska po utworzeniu piłki
            return ball;
        }
    }
}
