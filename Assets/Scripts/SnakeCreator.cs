using System;

namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    public class SnakeCreator : MonoBehaviour {

        [SerializeField] private SnakeScript _snakePrefab;
        [SerializeField] private SnakeCreatorUI _snakeCreatorUI;

        private List<KeyCode> _usedKeys = new List<KeyCode>();

        private List<SnakeScript> _snakeSpawned = new List<SnakeScript>();
        private List<SnakeScript> _deadSnakes = new List<SnakeScript>();

        void Update()
        {
            CheckInput();
            CheckForDeadSnakeRespawn();
        }

        private void CheckInput(){

            string feedback;
            feedback = "Precisone 2 teclas para criar uma cobra";
            _snakeCreatorUI.ChangeFeedback(feedback);       

            if (!Input.anyKey){               
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => !_usedKeys.Contains(key) && key != KeyCode.None)
                .ToArray();

           
            if(keysPressed.Length > 2){
                feedback = "Precisone somente 2 teclas";
               
                _snakeCreatorUI.ChangeFeedback(feedback);
                return;
            }

            if(keysPressed.Length == 1){

                feedback = string.Format("Tecla precionada: {0}. Precisone uma segunda tecla", keysPressed[0]);
               
                _snakeCreatorUI.ChangeFeedback(feedback);                
                return;
            }

            if(keysPressed.Length == 2){
                //TODO position
               var snake = Instantiate(_snakePrefab);
               snake.SetSnakeInput(keysPressed[1], keysPressed[0]);
               snake.OnSnakeDeath += OnSnakeDeath;
               snake.gameObject.name = string.Format("snake {0}", _snakeSpawned.Count);
               _snakeSpawned.Add(snake);

               foreach (var item in keysPressed)
               {
                   _usedKeys.Add(item);
               }
            }        
        }

        private void CheckForDeadSnakeRespawn(){
            if(_deadSnakes.Count == 0){
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => key != KeyCode.None);

            var tmpRespawnedSnakes = new List<SnakeScript>();

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
