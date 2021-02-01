using System;

namespace SnakeGame_Arvore_Test.Assets.Scripts
{
    using UnityEngine;
    
    public class SnakeBodyPart : MonoBehaviour {        
        private Animation _animation;
        public Vector3 LastPostion {get;set;}
        [SerializeField] private string _respawnAnimationName; 

        private Action<Collider> _onCollisionCallback;

        void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void SetOnCollisionCallback(Action<Collider> onCollision){
            _onCollisionCallback = onCollision;
        }

        void OnTriggerEnter(Collider other)
        {
            if(_onCollisionCallback != null){
                _onCollisionCallback(other);
            }
        }

        public void PlayerRespawnAnimation(){
            if(_animation == null){
                return;
            }

            _animation.Play(_respawnAnimationName);
        }

         public void StopRespawnAnimation(){
             if(_animation == null){
                return;
            }
           
            _animation.Rewind(_respawnAnimationName);   
            _animation.Play(_respawnAnimationName); 
            _animation.Sample();
            _animation.Stop(_respawnAnimationName);
           
        }


        void OnDestroy()
        {
            _onCollisionCallback = null;
        }

    }
}
