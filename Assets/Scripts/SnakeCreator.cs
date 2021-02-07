namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SnakeCreator{

        private SnakeScript _snakePrefab;

        private List<KeyCode> _usedKeys = new List<KeyCode>();

        private List<SnakeScript> _snakeSpawned = new List<SnakeScript>();
        private List<SnakeScript> _deadSnakes = new List<SnakeScript>();

        public List<SnakeScript> DeadSnakes {get{return _deadSnakes;}}
        private List<SnakeScript> SpawnedSnakes {get{return _snakeSpawned;}}
      

        public SnakeCreator(SnakeScript snakePrefab)
        {
            _snakePrefab = snakePrefab;
        }

        //TODO separar classe criar presenter
        //TODO fazer criar o bot tmb
        public void CheckInput(){           

            if (!Input.anyKey){               
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => !_usedKeys.Contains(key) && key != KeyCode.None)
                .ToArray();

            if (keysPressed.Length == 2)
            {
                //TODO position
                var snake = Object.Instantiate(_snakePrefab);
                snake.SetSnakeInput(keysPressed[1], keysPressed[0]);
                snake.OnSnakeDeath += OnSnakeDeath;
                snake.gameObject.name = string.Format("snake {0}", _snakeSpawned.Count);
                _snakeSpawned.Add(snake);

                for (int i = 0; i < 2; i++)
                {
                    _usedKeys.Add(keysPressed[i]);
                }
            }
        }

        private void CheckForDeadSnakeRespawn(){
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
                }
            }
        }

        private void OnSnakeDeath(SnakeScript deadSnake){
            
            if(_deadSnakes.Contains(deadSnake)){
                return;
            }
            _deadSnakes.Add(deadSnake);
        }
        
    }
}
