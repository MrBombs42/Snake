using System;

namespace SnakeGame.Assets.Scripts
{
   using UnityEngine;
   using UnityEngine.UI;
   
   public class SnakeCreatorUI : MonoBehaviour {
       
       [SerializeField] private Text _feedback;
       [SerializeField] private Text _respawnFeedback;


        public void ChangeFeedback(string feedback){
           if(!_feedback.gameObject.activeInHierarchy){
               _feedback.gameObject.SetActive(true);
           }

           _feedback.text = feedback;
       }

       public void EnableRespawnFeedback()
       {
           _respawnFeedback.gameObject.SetActive(true);
       }

       public void DisableRespawnFeedback()
       {
           _respawnFeedback.gameObject.SetActive(false);
       }


    }
}
