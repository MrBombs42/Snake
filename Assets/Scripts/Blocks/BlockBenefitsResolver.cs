using System;
using System.Collections.Generic;
using SnakeGame.Assets.Scripts.Blocks;

namespace SnakeGame.Assets.Scripts
{
    public static class BlockBenefitsResolver
    {
        private static readonly Dictionary<BlockTypes, IBlockBenefit> _mapper = new Dictionary<BlockTypes, IBlockBenefit>{
            {BlockTypes.EnginePower, new EnginePowerBlockBenefit()}
        };       

        public static void ResolveBlock(Block block, SnakeScript snake){
            var benefit = _mapper[block.BlockTypes];
            benefit.Resolve(snake);
        }
        
        
    }
}
