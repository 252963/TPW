using System.Collections.ObjectModel;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
    public class BallViewModel
    {
        public ObservableCollection<BallModel> Balls { get; private set; } = new();

        private readonly IBallLogic _logic;

        public BallViewModel(IBallLogic logic)
        {
            _logic = logic;
            foreach (var ball in _logic.Balls)
            {
                Balls.Add(new BallModel
                {
                    X = ball.X,
                    Y = ball.Y,
                    Radius = ball.Radius,
                    Color = ball.Color
                });
            }
        }

        public void Update(double deltaTime)
        {
            _logic.Update(deltaTime);
            int index = 0;
            foreach (var logicBall in _logic.Balls)
            {
                Balls[index].X = logicBall.X;
                Balls[index].Y = logicBall.Y;
                index++;
            }
        }
    }
}