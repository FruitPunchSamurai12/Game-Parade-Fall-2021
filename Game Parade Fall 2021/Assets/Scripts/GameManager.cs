using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Bakeneko catPrefab;
    [SerializeField]
    PlayerMovement playerPrefab;

    [SerializeField]
    Transform spawnCatPos;



    [SerializeField]
    Transform spawnPlayerPos;
    [SerializeField]
    ExitPoints[] exitPoints;

    public Transform PlayerTransform { get; private set; }
    public Transform CatTransform { get; private set; }
    public Transform ExitPoint { get { return exitPoints[_exitIndex].transform; } }
    public GameObject Bird { get; private set; }

    public GameObject Cat { get; private set; }

    [SerializeField] int birdLives=3;
    int currentBirdLives;


    public event Action onBirdCaught;
    public event Action onBirdReset;
    public event Action onBirdWon;
    public event Action onGameOver;

    bool _birdCaught = false;
    int _exitIndex;
    public static GameManager Instance { get; private set; } 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentBirdLives = birdLives;
        var player = Instantiate<PlayerMovement>(playerPrefab, spawnPlayerPos.position, Quaternion.identity);
        PlayerTransform = player.transform;
        Bird = player.gameObject;
        var neko = Instantiate<Bakeneko>(catPrefab, spawnCatPos.position, Quaternion.identity);
        CatTransform = neko.transform;
        Cat = neko.gameObject;
        Director.Instance.SetCatAndBird(Cat, Bird);
        _exitIndex = GameStateMachine.Instance.CurrentExit;
        for (int i = 0; i < exitPoints.Length; i++)
        {
            exitPoints[i].ToggleActive(_exitIndex == i);          
        }
        InputManager.Actions.Global.Pause.performed += HandlePausePressed;

    }

    private void OnDestroy()
    {
        InputManager.Actions.Global.Pause.performed -= HandlePausePressed;
    }

    private void HandlePausePressed(InputAction.CallbackContext obj)
    {
        GameStateMachine.Instance.PauseButtonPressed();
    }

    public void BirdGotCaught()
    {
        if (_birdCaught)
            return;
        currentBirdLives--;
        if(currentBirdLives<=0)
        {
            _birdCaught = true;
            onGameOver?.Invoke();
        }
        else
        {
            _birdCaught = true;
            onBirdCaught?.Invoke();
        }
    }

    public void ResetBird()
    {
        onBirdReset?.Invoke();
    }

    public void RespawnBird(Checkpoint currentCheckpoint)
    {
        _birdCaught = false;
        Bird.transform.position = currentCheckpoint.transform.position;
        Director.Instance.PlayerReset(currentCheckpoint.AreaID,currentCheckpoint.CatNewAreaID);
    }

    public void BirdGotToTheEnd()
    {
        onBirdWon?.Invoke();
    }
}
