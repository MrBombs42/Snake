﻿using System.Collections;
using System.Collections.Generic;
using SnakeGame_Arvore_Test.Assets.Scripts;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    private const int size = 1;

    [SerializeField] private SnakeBodyPart _snakeHeadPrefab;
    [SerializeField] private SnakeBodyPart _snakeTailPrefab;
    [SerializeField] private int _initialSnakeSize = 3;
    [SerializeField] private float _updateTime = 0.1f;


    [SerializeField] private bool _moveSnake = false;

    private Vector3 _movementDirection; 

    private SnakeBodyPart _snakeHead{
        get{
            return _snakeSegmentList[0];
        }
    }

    private List<SnakeBodyPart> _snakeSegmentList;

    void Start()
    {
        CreateSnake();
        _movementDirection = _snakeHead.transform.right;
    }


    private void CreateSnake(){
        Vector3 position = this.transform.position;
        _snakeSegmentList = new List<SnakeBodyPart>();

        for(int i = 0; i < _initialSnakeSize -1; i++){
            var tailInstance = Instantiate(_snakeTailPrefab, position, Quaternion.identity);
            tailInstance.LastPostion = position;
            tailInstance.transform.SetParent(this.transform);
            position.x += size;
            _snakeSegmentList.Add(tailInstance);
        }

        var headInstance = Instantiate(_snakeHeadPrefab, position, Quaternion.identity);
        headInstance.transform.SetParent(this.transform);
        _snakeSegmentList.Insert(0, headInstance);
    }
    private float _nextUpdate;

    void Update()
    {
        _nextUpdate += Time.deltaTime;
        if(_moveSnake && _nextUpdate > _updateTime){
            MoveSnake();
            _nextUpdate = 0;
        }
        UpdateDirection();

        if(Input.GetKeyDown(KeyCode.Space)){
            AddSegment();
        }
    }

    private void UpdateDirection(){
        if(Input.GetKeyDown(KeyCode.A)){
            _movementDirection = Quaternion.Euler(0,-90,0) * _movementDirection;
        }

        if(Input.GetKeyDown(KeyCode.D)){
            _movementDirection = Quaternion.Euler(0,90,0) * _movementDirection;
        }
    }

    private void MoveSnake(){
        _snakeHead.LastPostion = _snakeHead.transform.localPosition;
        _snakeHead.transform.localPosition += _movementDirection;
        for(int i = 1; i < _snakeSegmentList.Count; i++){
            var segment = _snakeSegmentList[i];
            segment.LastPostion = segment.transform.localPosition;        
            segment.transform.localPosition = _snakeSegmentList[i-1].LastPostion;
        }
    }

    private void AddSegment(){
        _snakeHead.LastPostion = _snakeHead.transform.localPosition;
        _snakeHead.transform.localPosition += _movementDirection;
        var tailInstance = Instantiate(_snakeTailPrefab, _snakeHead.LastPostion, Quaternion.identity);
        
        tailInstance.transform.SetParent(this.transform);
        tailInstance.LastPostion = _snakeSegmentList[1].transform.localPosition;
        _snakeSegmentList.Insert(1, tailInstance);
    }
}
