namespace Snake.Assets.Scripts
{
    using System;
    using System.Collections;
    using global::Assets.Scripts.Snake;
    using SnakeGame.Assets.Scripts;
    using UnityEngine;

    public class SnakeCreatorPresenter : MonoBehaviour {
        [SerializeField] private SnakeScript _snakePrefab;
        [SerializeField] private SnakeCreatorUI _snakeCreatorUI;
        [SerializeField] private BotSnake _botSnakePrefab;
        [SerializeField] private float _timeToSnakeBotRespawn = 2;


        private SnakeCreator _snakeCreator;

        void Awake()
        {
            _snakeCreator = new SnakeCreator(_snakePrefab, _botSnakePrefab);
        }

        void Start()
        {           
            TryDisableRespawnFeeback();
            var bot = _snakeCreator.CreateBotSnake();
            bot.OnSnakeDeath += OnBotSnakeDeath;
        }

        private void OnBotSnakeDeath(SnakeScript snake)
        {
            StartCoroutine(RespawnSnakeBot(snake));
        }

        private IEnumerator RespawnSnakeBot(SnakeScript snake)
        {
            yield return new WaitForSeconds(_timeToSnakeBotRespawn);

            snake.Respawn();
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
