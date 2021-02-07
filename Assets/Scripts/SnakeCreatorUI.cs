namespace SnakeGame.Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;
    using Snake.Assets.Scripts;
    using UnityEngine;
    using UnityEngine.UI;

    public class SnakeCreatorUI : MonoBehaviour {
       
       [SerializeField] private Text _feedback;
       [SerializeField] private Text _respawnFeedback;
       [SerializeField] private SnakeCreatorCellView _snakeCreatorCellviewPrefab;
       [SerializeField] private Transform _gridTransform;

       private List<SnakeCreatorCellView> _cellViews = new List<SnakeCreatorCellView>();


        public void AddSnakeCell(SnakeScript snake){
            var cellView = Instantiate(_snakeCreatorCellviewPrefab);
            cellView.transform.SetParent(_gridTransform);
            cellView.ConfigureForSnake(snake);
            _cellViews.Add(cellView);
            cellView.SetTextColor(Color.green);
        }

        public void DisableSnakeCell(SnakeScript snake){
            var cellView = _cellViews.FirstOrDefault(s => s.PlayerId == snake.GetInstanceID());
            if(cellView){
                cellView.SetTextColor(Color.red);
            }
        }

        public void EnableSnakeCell(SnakeScript snake){
            var cellView = _cellViews.FirstOrDefault(s => s.PlayerId == snake.GetInstanceID());
            if(cellView){
                cellView.SetTextColor(Color.green);
            }
        }

       public void CreateSnakeFeedback()
       {
           TryEnableFeedback();

           _feedback.text = "Precisone 2 teclas para criar uma cobra";
        }

       public void PressOneMoreKeyFeedback(string key)
       {
           TryEnableFeedback();

           _feedback.text = string.Format("Tecla precionada: {0}. Precisone uma segunda tecla n�o utilizada", key);

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
