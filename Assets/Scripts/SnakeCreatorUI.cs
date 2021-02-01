using System;

namespace SnakeGame.Assets.Scripts
{
   using UnityEngine;
   using UnityEngine.UI;
   
   public class SnakeCreatorUI : MonoBehaviour {
       
       [SerializeField] private Text _feedback;


       public void ChangeFeedback(string feedback){
           if(!_feedback.gameObject.activeInHierarchy){
               _feedback.gameObject.SetActive(true);
           }

           _feedback.text = feedback;
       }
       
   }
}
