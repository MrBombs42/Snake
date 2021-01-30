using System.Collections;
using System.Collections.Generic;
using SnakeGame_Arvore_Test.Assets.Scripts;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    private const int size = 1;

    [SerializeField] private SnakeBodyPart _snakeHeadPrefab;
    [SerializeField] private SnakeBodyPart _snakeTailPrefab;
    [SerializeField] private int _initialSnakeSize = 3;

    private List<SnakeBodyPart> _snakeSegmentList;

    void Start()
    {
        CreateSnake();
    }


    private void CreateSnake(){
        Vector3 position = this.transform.position;
        _snakeSegmentList = new List<SnakeBodyPart>();

        for(int i = 0; i < _initialSnakeSize -1; i++){
            var tailInstance = Instantiate(_snakeTailPrefab, position, Quaternion.identity);
            tailInstance.transform.SetParent(this.transform);
            position.x += size;
            _snakeSegmentList.Add(tailInstance);
        }

        var headInstance = Instantiate(_snakeHeadPrefab, position, Quaternion.identity);
        headInstance.transform.SetParent(this.transform);
        _snakeSegmentList.Add(headInstance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
