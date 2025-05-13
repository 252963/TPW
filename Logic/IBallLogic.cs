using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic
{
    public interface IBallLogic
    {
        IEnumerable<IBall> Balls { get; }
        void CreateBalls(int count, double width, double height);
        void Update();
        void UpdateBounds(double width, double height);
    }
}
