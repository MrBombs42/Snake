using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using SnakeGame.Assets.Scripts;
using SnakeGame_Arvore_Test.Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

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
    protected Vector3 _movementDirection; 
    private float _updateTime;
    private int _batteringRamBlocks = 0;
    private TimeTravelStatusHolder _timeTravelStatusHolder;
    private float _nextUpdate;
    private Color _tailColor = Color.green;

    protected SnakeBodyPart _snakeHead{
        get{
            return _snakeSegmentList[0];
        }
    }

    private List<SnakeBodyPart> _snakeSegmentList;

    protected virtual void Start()
    {
        InitializeDefaultSnake(_initialSnakeSize, transform.position, true);
        _moveSnake = true;
    }

    private void CreateSnake(int snakeSize, Vector3 position, bool changeColor){
       
        _snakeSegmentList = new List<SnakeBodyPart>();

        if (changeColor)
        {
            _tailColor = Random.ColorHSV();
        }

        for(int i = 0; i < snakeSize - 1; i++){
            var tailInstance = Instantiate(_snakeTailPrefab, position, Quaternion.identity);
            tailInstance.LastPostion = position;
            tailInstance.SetOnCollisionCallback(OnTailCollision);
            tailInstance.transform.SetParent(this.transform);
            position.x += size;
            tailInstance.SetColor(_tailColor);
            _snakeSegmentList.Add(tailInstance);
        }

        var headInstance = Instantiate(_snakeHeadPrefab, position, Quaternion.identity);
        headInstance.SetOnCollisionCallback(OnHeadCollision);
        headInstance.transform.SetParent(this.transform);
        _snakeSegmentList.Insert(0, headInstance);
    }

    private void OnHeadCollision(Collider collision, SnakeBodyPart myBodyPart)
    {
        if(CheckForSelfCollision(collision)){
            SnakeDeath();
        }

        Block block;
        if(CheckCollisionWithBlock(collision, out block)){
            AddSegment();
            BlockBenefitsResolver.ResolveBlock(block, this);
            return;
        }

        SnakeScript otherSnake;
        if (CheckCollisionWithOtherSnake(collision, out otherSnake))
        {
            if (HasBatteringRamEnabled())
            {
                StartCoroutine(DecreaseBatteringRam());
            }
            else
            {
                SnakeDeath();
            }
        }
    }

    private void OnTailCollision(Collider collision, SnakeBodyPart myBodyPart)
    {
        if(CheckForSelfCollision(collision)){
           return;
        }

        SnakeScript otherSnake;
        if (CheckCollisionWithOtherSnake(collision, out otherSnake))
        {
            if (otherSnake.HasBatteringRamEnabled())
            {
                _snakeSegmentList.Remove(myBodyPart);
                Destroy(myBodyPart.gameObject);
            }
        }
    }

    private IEnumerator DecreaseBatteringRam()
    {
        yield return new WaitForSeconds(1);//todo check for head collision end
        _batteringRamBlocks--;
    }

    private bool CheckCollisionWithOtherSnake(Collider other, out SnakeScript snake)
    {
        snake = other.transform.parent.GetComponent<SnakeScript>();
        return snake != null;
    }

    private bool CheckCollisionWithBlock(Collider other, out Block block){
        block = other.gameObject.GetComponent<Block>();
        return block != null;
    }

    private void SnakeDeath(){
        _moveSnake = false;
        this.gameObject.SetActive(false);
        foreach (var segment in _snakeSegmentList)
        {           
            Destroy(segment.gameObject);
        }
        _snakeSegmentList.Clear();

        if (_timeTravelStatusHolder != null)
        {
            TimeTravel();
            return;
        }

        if(OnSnakeDeath != null){
            OnSnakeDeath(this);
        }
    }

    private void TimeTravel()
    {
        InitializeDefaultSnake(_timeTravelStatusHolder.SnakeSize, _timeTravelStatusHolder.Position, false);
        this.gameObject.SetActive(true);
        //TODO change feedback
        //foreach (var segment in _snakeSegmentList)
        //{
        //    segment.PlayerRespawnAnimation();
        //}

        //TODO animation
        StartCoroutine(StartMovingAfterRespawn());

        _timeTravelStatusHolder = null;
    }

    public void AddTimeTravel()
    {
        _timeTravelStatusHolder = new TimeTravelStatusHolder
        {
            Position = _snakeHead.transform.localPosition,
            SnakeSize = _snakeSegmentList.Count
        };
    }

    public bool HasBatteringRamEnabled()
    {
        return _batteringRamBlocks > 0;
    }

    public void AddBatteringRamBlock()
    {
        _batteringRamBlocks++;
    }

    public void Respawn(){

        this.gameObject.SetActive(true);
        InitializeDefaultSnake(_initialSnakeSize, transform.position, false);

        //foreach (var segment in _snakeSegmentList)
        //{
        //    segment.PlayerRespawnAnimation();
        //}
        
        //TODO animation
        StartCoroutine(StartMovingAfterRespawn());
    }

    private void InitializeDefaultSnake(int snakeSize, Vector3 position, bool changeColor){
        CreateSnake(snakeSize, position, changeColor);
        _movementDirection = Vector3.right;
        _updateTime = _baseUpdateTime;
    }

    private IEnumerator StartMovingAfterRespawn(){
        yield return new WaitForSeconds(_starMovingInSecondsAfterRespawn);
        _moveSnake = true;
        //foreach (var segment in _snakeSegmentList)
        //{
        //    segment.StopRespawnAnimation();
        //}
    }

    private bool CheckForSelfCollision(Collider otherCollider){       
        return otherCollider.transform.parent == this.transform;
    }

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

    protected virtual void MoveSnake(){
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
            SetHeadNewPosition(new Vector3(snakeXPosition, 0, -boardSize));
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
        tailInstance.SetColor(_tailColor);

        _snakeSegmentList.Insert(1, tailInstance);
        _updateTime += _loadIncrease;
        _updateTime = Mathf.Min(_updateTime, 1);
    }
}
