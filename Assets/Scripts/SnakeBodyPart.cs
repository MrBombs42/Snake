using System;

namespace SnakeGame_Arvore_Test.Assets.Scripts
{
    using UnityEngine;
    
    public class SnakeBodyPart : MonoBehaviour {
        
        public Vector3 LastPostion{get;set;}

        private Action<Collider> _onCollisionCallback;

        public void SetOnCollisionCallback(Action<Collider> onCollision){
            _onCollisionCallback = onCollision;
        }

        void OnTriggerEnter(Collider other)
        {
            if(_onCollisionCallback != null){
                _onCollisionCallback(other);
            }
        }

    }
}
