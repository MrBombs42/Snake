using SnakeGame.Assets.Scripts.Blocks;

namespace Assets.Scripts.Blocks
{
    public class TimeTravelBlockBenefit : IBlockBenefit
    {
        public void Resolve(SnakeScript snake)
        {
            snake.AddTimeTravel();
        }
    }
}
