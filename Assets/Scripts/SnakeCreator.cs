namespace SnakeGame.Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Assets.Scripts.Snake;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class SnakeCreator{

        public event Action<SnakeScript> OnSnakeDeathEvt;
        public event Action<SnakeScript> OnSnakeRespawnEvt;
        public event Action<SnakeScript> OnSnakeCreatedEvt;

        private SnakeScript _snakePrefab;
        private BotSnake _botSnakePrefab;

        private List<KeyCode> _usedKeys = new List<KeyCode>();

        private List<SnakeScript> _snakeSpawned = new List<SnakeScript>();
        private List<SnakeScript> _deadSnakes = new List<SnakeScript>();

        public List<SnakeScript> DeadSnakes { get{ return _deadSnakes;}}
        private List<SnakeScript> SpawnedSnakes { get{ return _snakeSpawned;}}
      

        public SnakeCreator(SnakeScript snakePrefab, BotSnake botSnakePrefab)
        {
            _snakePrefab = snakePrefab;
            _botSnakePrefab = botSnakePrefab;
        }

        public void VerifyInputAndTryCreateNewSnake(){           

            if (!Input.anyKey){               
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => !_usedKeys.Contains(key) && key != KeyCode.None)
                .ToArray();

            if (keysPressed.Length == 2)
            {
                var snake = CreateNewSnake(keysPressed);             
                
                for (int i = 0; i < 2; i++)
                {
                    _usedKeys.Add(keysPressed[i]);
                }

                if(OnSnakeCreatedEvt != null){
                    OnSnakeCreatedEvt(snake);
                }
            }
        }

        private SnakeScript CreateNewSnake(KeyCode[] keysPressed){

            var snake = Object.Instantiate(_snakePrefab);
            snake.SetSnakeInput(keysPressed[1], keysPressed[0]);
            snake.OnSnakeDeath += OnSnakeDeath;
            snake.gameObject.name = string.Format("snake {0}", _snakeSpawned.Count);
            _snakeSpawned.Add(snake);
            return snake;
        }

        public BotSnake CreateBotSnake(){

            var bot = Object.Instantiate(_botSnakePrefab);           
            bot.gameObject.name = "BotSnake";
            bot.OnSnakeDeath += OnSnakeDeath;
            _snakeSpawned.Add(bot);
            return bot;
        }

        public void CheckForDeadSnakeRespawn(){
            if(_deadSnakes.Count == 0){
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => key != KeyCode.None);
            
            for(int i = 0; i < _deadSnakes.Count; i++){
                var snake = _deadSnakes[i];
                var keycodePair = snake.KeyCodePair;
                if(keysPressed.Contains(keycodePair.LeftKey) &&
                    keysPressed.Contains(keycodePair.RightKey))
                {
                    snake.Respawn();
                    _deadSnakes.Remove(snake);
                    i--;
                    if(OnSnakeRespawnEvt != null){
                        OnSnakeRespawnEvt(snake);
                    }
                }
            }
        }

        private void OnSnakeDeath(SnakeScript deadSnake){
            
            if(_deadSnakes.Contains(deadSnake)){
                return;
            }
            _deadSnakes.Add(deadSnake);

            if(OnSnakeDeathEvt != null){
                OnSnakeDeathEvt(deadSnake);
            }
        }
        
    }
}
