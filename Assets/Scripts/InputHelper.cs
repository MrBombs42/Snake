using System;

namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;   
    
    public class InputHelper{
       private static readonly KeyCode[] _enabledKeys = {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,        
            KeyCode.S,        
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
        };


        public static KeyCode GetKeyPressed(){

            if(!Input.anyKeyDown){
                return KeyCode.None;
            }

            for(int i =0; i < _enabledKeys.Length; i++){
                var key = _enabledKeys[i];
                if(Input.GetKeyDown(key)){
                    return key;
                }
            }

            return KeyCode.None;
        }

        public static IEnumerable<KeyCode> GetCurrentKeysDown()
        {
            if (Input.anyKeyDown)
            for (int i = 0; i < _enabledKeys.Length; i++){
                if (Input.GetKeyDown(_enabledKeys[i])){
                    yield return _enabledKeys[i];
                }                   
            }                
        }

        public static IEnumerable<KeyCode> GetCurrentKeys()
        {
            if (Input.anyKey){
                for (int i = 0; i < _enabledKeys.Length; i++){
                    if (Input.GetKey(_enabledKeys[i])){
                        yield return _enabledKeys[i];
                    }
                }  
            }                       
                        
        }

    }
}
