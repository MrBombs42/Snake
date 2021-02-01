using System;

namespace SnakeGame.Assets.Scripts.Blocks
{
    public class EnginePowerBlockBenefit : IBlockBenefit
    {
        private const float LoadDecrease = 0.03f;//TODO get from config
        public void Resolve(SnakeScript snake)
        {
            snake.DecreateLoad(LoadDecrease);
        }
    }
}
