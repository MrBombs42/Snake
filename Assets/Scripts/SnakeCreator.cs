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
        void Start()
        {
        }

        void Update()
        {
            CheckInput();
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

               foreach (var item in keysPressed)
               {
                   _usedKeys.Add(item);
               }
            }        
        }
        
    }
}
