using System;

namespace SnakeGame.Assets.Scripts
{
    using UnityEngine;
    
    public class Block : MonoBehaviour {
        
        public event Action<Block> OnDestroyed;

        void OnTriggerEnter(Collider other)
        {            
            Destroy(this.gameObject); 
            if(OnDestroyed != null){
                OnDestroyed(this);
            }               
        }
    }
}
