using System;

namespace Snake.Assets.Scripts
{
    using SnakeGame.Assets.Scripts;
    using UnityEngine;
  
  public class SnakeCreatorPresenter : MonoBehaviour {
       [SerializeField] private SnakeScript _snakePrefab;
        [SerializeField] private SnakeCreatorUI _snakeCreatorUI;


        private SnakeCreator _snakeCreator;

        void Awake()
        {
            _snakeCreator = new SnakeCreator(_snakePrefab);
        }

        void Start()
        {           

            TryDisableRespawnFeeback();
        }

        void Update()
        {
            _snakeCreator.CheckInput();
            TryDisableRespawnFeeback();
        }        

        private void TryDisableRespawnFeeback()
        {
            if (_snakeCreator.DeadSnakes.Count == 0)
            {
                _snakeCreatorUI.DisableRespawnFeedback();
            }
        }
  }
}
