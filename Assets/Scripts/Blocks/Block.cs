using System;

namespace SnakeGame.Assets.Scripts
{
    using SnakeGame.Assets.Scripts.Blocks;
    using UnityEngine;
    
    public class Block : MonoBehaviour {        
        public event Action<Block> OnDestroyed;
        public BlockTypes BlockTypes;

        void OnTriggerEnter(Collider other)
        {            
            Destroy(this.gameObject); 
            if(OnDestroyed != null){
                OnDestroyed(this);
            }               
        }
    }
}
