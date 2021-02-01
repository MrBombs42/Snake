using System;

namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public class BlockSpawner : MonoBehaviour {
        
        [SerializeField] private Block[] _blockTypesPrefab;

        private List<Block> _spawnedBlocks = new List<Block>();


        void Start()
        {
            GeneraterandomBlock();
        }

        private void GeneraterandomBlock(){
            var block = CreateBlock(GetRandomPosition());
            block.OnDestroyed += OnBlockGotDestroyed;
            _spawnedBlocks.Add(block);
        }

        private void OnBlockGotDestroyed(Block block){
            block.OnDestroyed -= OnBlockGotDestroyed;
            _spawnedBlocks.Remove(block);
            GeneraterandomBlock();
        }


        private Block CreateBlock(Vector3 position)
        {
            var index = RandomBlockIndex();
            var block = _blockTypesPrefab[index];
            return Instantiate(block, position, block.transform.rotation);
        }

        private Vector3 GetRandomPosition(){
            var boardSize = GameBoard.BoardSize;
            var posX = Random.Range(-boardSize, boardSize);
            var posZ =  Random.Range(-boardSize, boardSize);

            return new Vector3(posX, 0, posZ);
        }

        private int RandomBlockIndex()
        {
            return Random.Range(0, _blockTypesPrefab.Length);
        }
        
    }
}
