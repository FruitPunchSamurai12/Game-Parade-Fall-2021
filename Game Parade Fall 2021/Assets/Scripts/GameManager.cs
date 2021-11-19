using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Transform exitPoint;

    public Transform PlayerTransform { get; private set; }
    public Transform CatTransform { get; private set; }
    public Transform ExitPoint { get { return exitPoint; } }
    public GameObject Bird { get; private set; }

    public GameObject Cat { get; private set; }

    [SerializeField] int birdLives=3;
    int currentBirdLives;


    public event Action onBirdCaught;
    public event Action onGameOver;

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
        Director.Instance.SetCatAndBird(neko.GetComponent<CellTracker>(), player.GetComponent<CellTracker>());
    }

    public void BirdGotCaught()
    {
        currentBirdLives--;
        if(currentBirdLives<=0)
        {
            onGameOver?.Invoke();
        }
        else
        {
            onBirdCaught?.Invoke();
        }
    }


    public void RespawnBird(Checkpoint currentCheckpoint)
    {
        Bird.GetComponent<CellTracker>().Teleport(currentCheckpoint.transform.position);
        Director.Instance.TeleportCat(currentCheckpoint.waypointToTeleportCatAfterItCatchesTheBird);
    }

}
