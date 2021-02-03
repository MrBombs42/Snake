using System;

namespace SnakeGame.Assets.Scripts
{
   using UnityEngine;
   using UnityEngine.UI;
   
   public class SnakeCreatorUI : MonoBehaviour {
       
       [SerializeField] private Text _feedback;
       [SerializeField] private Text _respawnFeedback;


       public void DefaultFeedback()
       {
           TryEnableFeedback();

           _feedback.text = "Precisone 2 teclas para criar uma cobra";
        }

       public void PressOneMoreKeyFeedback(string key)
       {
           TryEnableFeedback();

           _feedback.text = string.Format("Tecla precionada: {0}. Precisone uma segunda tecla não utilizada", key);

        }

        public void ChangeFeedback(string feedback)
        {
            TryEnableFeedback();

            _feedback.text = feedback;
        }

        private void TryEnableFeedback()
        {
            if (!_feedback.gameObject.activeInHierarchy)
            {
                _feedback.gameObject.SetActive(true);
            }
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
