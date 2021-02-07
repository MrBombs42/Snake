namespace Snake.Assets.Scripts
{
    using global::Assets.Scripts.Snake;
    using SnakeGame.Assets.Scripts;
    using UnityEngine;

    public class SnakeCreatorPresenter : MonoBehaviour {
        [SerializeField] private SnakeScript _snakePrefab;
        [SerializeField] private SnakeCreatorUI _snakeCreatorUI;
        [SerializeField] private BotSnake _botSnakePrefab;


        private SnakeCreator _snakeCreator;

        void Awake()
        {
            _snakeCreator = new SnakeCreator(_snakePrefab, _botSnakePrefab);
        }

        void Start()
        {           
            TryDisableRespawnFeeback();
            _snakeCreator.CreateBotSnake();
        }

        void Update()
        {
            _snakeCreator.CheckInput();
            _snakeCreator.CheckForDeadSnakeRespawn();
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
