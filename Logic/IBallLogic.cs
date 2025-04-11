using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic
{
    public interface IBallLogic
    {
        List<IBall> CreateBalls(int count);
        void MoveBall(IBall ball);
    }
}