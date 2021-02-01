using System;
using System.Collections;
using System.Collections.Generic;
using SnakeGame.Assets.Scripts;
using SnakeGame_Arvore_Test.Assets.Scripts;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{    
    private const int size = 1;

    public event Action<SnakeScript> OnSnakeDeath;

    [SerializeField] private SnakeBodyPart _snakeHeadPrefab;
    [SerializeField] private SnakeBodyPart _snakeTailPrefab;
    [SerializeField] private int _initialSnakeSize = 3;
    [SerializeField] private float _baseUpdateTime = 0.1f;
    [SerializeField] private float _loadIncrease = 0.03f;
    [SerializeField] private bool _moveSnake = false;
    [SerializeField] private float _starMovingInSecondsAfterRespawn = 1;

    public PlayerKeyCodePair KeyCodePair{get{return _keyCodePair;}}

    private PlayerKeyCodePair _keyCodePair;
    private Vector3 _movementDirection; 
    private float _updateTime;

    private SnakeBodyPart _snakeHead{
        get{
            return _snakeSegmentList[0];
        }
    }

    private List<SnakeBodyPart> _snakeSegmentList;

    void Start()
    {
        InitializeDefaultSnake();
    }

    private void CreateSnake(){
        Vector3 position = this.transform.position;
        _snakeSegmentList = new List<SnakeBodyPart>();

        for(int i = 0; i < _initialSnakeSize -1; i++){
            var tailInstance = Instantiate(_snakeTailPrefab, position, Quaternion.identity);
            tailInstance.LastPostion = position;
            tailInstance.SetOnCollisionCallback(OnTailCollision);
            tailInstance.transform.SetParent(this.transform);
            position.x += size;
            _snakeSegmentList.Add(tailInstance);
        }

        var headInstance = Instantiate(_snakeHeadPrefab, position, Quaternion.identity);
        headInstance.SetOnCollisionCallback(OnHeadCollision);
        headInstance.transform.SetParent(this.transform);
        _snakeSegmentList.Insert(0, headInstance);
    }

    private void OnHeadCollision(Collider collision){

        if(CheckForSelfCollision(collision)){
            Debug.LogError("Mori");
            SnakeDeath();
        }

        Block block;
        if(CheckCollisionWithBlock(collision, out block)){
            AddSegment();
            BlockBenefitsResolver.ResolveBlock(block, this);
        }

    }

    private void OnTailCollision(Collider collision){

        if(!CheckForSelfCollision(collision)){
           return;
        }
    }

    private bool CheckCollisionWithBlock(Collider other, out Block block){
        block = other.gameObject.GetComponent<Block>();
        return block != null;
    }

    private void SnakeDeath(){

        //TODO check for Battering ram

        _moveSnake = false;
        this.gameObject.SetActive(false);
        foreach (var segment in _snakeSegmentList)
        {           
            Destroy(segment.gameObject);
        }
        _snakeSegmentList.Clear();

        if(OnSnakeDeath != null){
            OnSnakeDeath(this);
        }
    }

    public void Respawn(){
        //TODO check for TimeTravel
        this.gameObject.SetActive(true);
        InitializeDefaultSnake();

        foreach (var segment in _snakeSegmentList)
        {
            segment.PlayerRespawnAnimation();
        }
        
        //TODO animation
        StartCoroutine(StartMovingAfterRespawn());
    }

    private void InitializeDefaultSnake(){
        CreateSnake();
        _movementDirection = Vector3.right;
        _updateTime = _baseUpdateTime;
    }

    private IEnumerator StartMovingAfterRespawn(){
        yield return new WaitForSeconds(_starMovingInSecondsAfterRespawn);
        _moveSnake = true;
        foreach (var segment in _snakeSegmentList)
        {
            segment.StopRespawnAnimation();
        }
    }

    private bool CheckForSelfCollision(Collider otherCollider){       
        return otherCollider.transform.parent == this.transform;
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

    public void SetSnakeInput(KeyCode rigthInput, KeyCode leftInput){
        _keyCodePair.RightKey = rigthInput;
        _keyCodePair.LeftKey = leftInput;
    }

    private void UpdateDirection(){
        if(Input.GetKeyDown(_keyCodePair.LeftKey)){
            _movementDirection = Quaternion.Euler(0,-90,0) * _movementDirection;
        }

        if(Input.GetKeyDown(_keyCodePair.RightKey)){
            _movementDirection = Quaternion.Euler(0,90,0) * _movementDirection;
        }
    }

    private void MoveSnake(){
        _snakeHead.LastPostion = _snakeHead.transform.localPosition;
        _snakeHead.transform.localPosition += _movementDirection;

        CheckForBoardReach();

        for(int i = 1; i < _snakeSegmentList.Count; i++){
            var segment = _snakeSegmentList[i];
            segment.LastPostion = segment.transform.localPosition;        
            segment.transform.localPosition = _snakeSegmentList[i-1].LastPostion;
        }
    }

    private void CheckForBoardReach(){
        var snakeXPosition = _snakeHead.transform.localPosition.x;
        var snakeZPosition = _snakeHead.transform.localPosition.z;
        var boardSize =  GameBoard.BoardSize;

        if(snakeXPosition > boardSize){
            SetHeadNewPosition(new Vector3(-boardSize, 0, snakeZPosition));
            return;
        }

        if(snakeXPosition < -boardSize){
            SetHeadNewPosition(new Vector3(boardSize, 0, snakeZPosition));
            return;
        }

        if(snakeZPosition < -boardSize){
            SetHeadNewPosition(new Vector3(snakeXPosition, 0, boardSize));
            return;
        }

        if(snakeZPosition > boardSize){
            SetHeadNewPosition(new Vector3(-snakeXPosition, 0, -boardSize));
            return;
        }
    }

    public void DecreateLoad(float decreaseNumber){
        _updateTime -= decreaseNumber;
        _updateTime = Mathf.Max(_updateTime, 0.01f);
    }

    private void SetHeadNewPosition(Vector3 newPosition){
        _snakeHead.transform.localPosition = newPosition;
    }

    private void AddSegment(){
        _snakeHead.LastPostion = _snakeHead.transform.localPosition;
        _snakeHead.transform.localPosition += _movementDirection;
        var tailInstance = Instantiate(_snakeTailPrefab, _snakeHead.LastPostion, Quaternion.identity);
        
        tailInstance.transform.SetParent(this.transform);
        tailInstance.LastPostion = _snakeSegmentList[1].transform.localPosition;
        tailInstance.SetOnCollisionCallback(OnTailCollision);
        _snakeSegmentList.Insert(1, tailInstance);
        _updateTime += _loadIncrease;
    }
}
