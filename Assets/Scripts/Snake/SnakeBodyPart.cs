using System;

namespace SnakeGame_Arvore_Test.Assets.Scripts
{
    using UnityEngine;
    
    public class SnakeBodyPart : MonoBehaviour {
        public Vector3 LastPostion {get;set;}

        private Action<Collider, SnakeBodyPart> _onCollisionCallback;

        public void SetOnCollisionCallback(Action<Collider, SnakeBodyPart> onCollision){
            _onCollisionCallback = onCollision;
        }

        public void SetColor(Color color)
        {
            var meshRender = GetComponent<MeshRenderer>();
            meshRender.material.color = color;
        }

        void OnTriggerEnter(Collider other)
        {
            if(_onCollisionCallback != null){
                _onCollisionCallback(other, this);
            }
        }


        void OnDestroy()
        {
            _onCollisionCallback = null;
        }

    }
}
