using Assets.Scripts.Snake;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "SnakePrefabsCreator", menuName = "ScriptableObjects/SnakePrefabsCreatorScriptableObject", order = 1)]
    public class SnakePrefabsCreatorScriptableObject : ScriptableObject
    {
        [SerializeField] private SnakeScript _snakePrefab;
        [SerializeField] private BotSnake _botSnakePrefab;

        public SnakeScript SnakePrefab
        {
            get { return _snakePrefab; }
        }
        public BotSnake BotSnakePrefab
        {
            get { return _botSnakePrefab; }
        }
    }
}
