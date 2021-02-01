using System;

namespace SnakeGame.Assets.Scripts.Blocks
{
    public interface IBlockBenefit
    {
        void Resolve(SnakeScript snake);
    }
}
