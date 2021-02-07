namespace Snake.Assets.Scripts
{
    using System;
    using System.Collections;
    using global::Assets.Scripts.Snake;
    using SnakeGame.Assets.Scripts;
    using UnityEngine;

    public class SnakeCreatorPresenter : MonoBehaviour {
        [SerializeField] private SnakeScript _snakePrefab;//TODO get from a scriptableobject
        [SerializeField] private SnakeCreatorUI _snakeCreatorUI;
        [SerializeField] private BotSnake _botSnakePrefab;
        [SerializeField] private float _timeToSnakeBotRespawn = 2;

        private SnakeCreator _snakeCreator;

        void Awake()
        {
            _snakeCreator = new SnakeCreator(_snakePrefab, _botSnakePrefab);
            _snakeCreator.OnSnakeDeathEvt += OnSnakeDeath;
            _snakeCreator.OnSnakeRespawnEvt += OnSnakeRespawn;
            _snakeCreator.OnSnakeCreatedEvt += OnSnakeCreated;
        }

        void OnDestroy()
        {
            _snakeCreator.OnSnakeDeathEvt -= OnSnakeDeath;
            _snakeCreator.OnSnakeRespawnEvt -= OnSnakeRespawn;
            _snakeCreator.OnSnakeCreatedEvt -= OnSnakeCreated;
        }

        void Start()
        {           
            TryDisableRespawnFeeback();
            var bot = _snakeCreator.CreateBotSnake();
            bot.OnSnakeDeath += OnBotSnakeDeath;
            _snakeCreatorUI.AddSnakeCell(bot);

            _snakeCreatorUI.CreateSnakeFeedback();
        }

        private void OnBotSnakeDeath(SnakeScript snake)
        {
            StartCoroutine(RespawnSnakeBot(snake));
            _snakeCreatorUI.DisableSnakeCell(snake);
        }

        private IEnumerator RespawnSnakeBot(SnakeScript snake)
        {
            yield return new WaitForSeconds(_timeToSnakeBotRespawn);

            snake.Respawn();
            _snakeCreatorUI.EnableSnakeCell(snake);
        }

        void Update()
        {
            _snakeCreator.VerifyInputAndTryCreateNewSnake();
            _snakeCreator.CheckForDeadSnakeRespawn();
        }        

        private void TryDisableRespawnFeeback()
        {
            if (_snakeCreator.DeadSnakes.Count == 0)
            {
                _snakeCreatorUI.DisableRespawnFeedback();
            }
        }     
           
        private void OnSnakeCreated(SnakeScript snake)
        {
            _snakeCreatorUI.AddSnakeCell(snake);
        }

        private void OnSnakeRespawn(SnakeScript snake)
        {
            _snakeCreatorUI.EnableSnakeCell(snake);
            TryDisableRespawnFeeback();
        }

        private void OnSnakeDeath(SnakeScript snake)
        {
            _snakeCreatorUI.DisableSnakeCell(snake);
            _snakeCreatorUI.EnableRespawnFeedback();
        }
  }
}
