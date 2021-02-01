using System;
using SnakeGame.Assets.Scripts.Blocks;

namespace Assets.Scripts.Blocks
{
    public class BatteringRamBlockBenefit : IBlockBenefit
    {
        public void Resolve(SnakeScript snake)
        {
            snake.AddBatteringRamBlock();
        }
    }
}
