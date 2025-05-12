using System;
using System.Windows;

namespace TPW.Presentation.View
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