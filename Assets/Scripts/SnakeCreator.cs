using System;

namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    public class SnakeCreator : MonoBehaviour {

        [SerializeField] private SnakeScript _snakePrefab;

        private List<KeyCode> _usedKeys = new List<KeyCode>();
        void Start()
        {
        }

        void Update()
        {
            CheckInput();
        }


        private void CheckInput(){
            if (!Input.anyKey){
                return;
            }

            var keysPressed = InputHelper.GetCurrentKeys()
                .Where(key => !_usedKeys.Contains(key) && key != KeyCode.None)
                .ToArray();


            if(keysPressed.Length > 2){
                Debug.LogError("Precisone somente 2 teclas");
                return;
            }

            if(keysPressed.Length == 1){
                Debug.LogError("Precisone uma segunda tecla");
                return;
            }

            if(keysPressed.Length == 2){
                //TODO position
               var snake = Instantiate(_snakePrefab);
               snake.SetSnakeInput(keysPressed[0], keysPressed[1]);

               foreach (var item in keysPressed)
               {
                   _usedKeys.Add(item);
               }
            }
        
        }
        
    }
}
