namespace Snake.Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class SnakeCreatorCellView : MonoBehaviour {
        [SerializeField] private Text _playerName;
        [SerializeField] private Text _playerKeys;

        private int _playerId;

        public int PlayerId { get{ return _playerId;}}

        public void ConfigureForSnake(SnakeScript snake){
            _playerId = snake.GetInstanceID();
            _playerName.text = snake.name;
            var keys = snake.KeyCodePair;
            _playerKeys.text = string.Format("{0} + {1}", keys.LeftKey, keys.RightKey);
        }

        public void SetTextColor(Color color){
            _playerKeys.color = color;
            _playerName.color = color;
        }
    }
}
