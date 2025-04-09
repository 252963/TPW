using System;
using System.Windows;
using TPW.Presentation;

namespace Presentation.View
{
    public class BallApp : Application
    {
        [STAThread]
        public static void Main()
        {
            var app = new BallApp();
            app.Run(new MainWindow());
        }
    }
}