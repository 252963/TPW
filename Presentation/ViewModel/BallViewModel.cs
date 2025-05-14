using System.Collections.ObjectModel;
using TPW.Data;
using TPW.Logic;
using TPW.Presentation.Model;

namespace TPW.Presentation.ViewModel
{
    public class BallViewModel
    {
        public ObservableCollection<BallModel> Balls { get; } = new();
        private readonly IBallLogic _logic;

        public BallViewModel(IBallLogic logic)
        {
            _logic = logic;
            RefreshBalls();
        }

        public void RefreshBalls()
        {
            Balls.Clear();

            foreach (var ball in _logic.Balls)
            {
                var model = new BallModel
                {
                    X = ball.X,
                    Y = ball.Y,
                    Radius = ball.Radius,
                    Color = ball.Color
                };

                ball.PositionChanged += (s, e) =>
                {
                    var (x, y) = ball.GetPosition();
                    model.X = x;
                    model.Y = y;
                };

                Balls.Add(model);
            }
        }
    }
}