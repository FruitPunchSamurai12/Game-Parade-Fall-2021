using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Bakeneko catPrefab;
    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    Transform spawnCatPos;
    [SerializeField]
    Transform spawnPlayerPos;

    public Transform PlayerTransform { get; private set; }
    public Transform CatTransform { get; private set; }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {   
        var player = Instantiate(playerPrefab, spawnPlayerPos.position, Quaternion.identity);
        PlayerTransform = player.transform;
        var neko = Instantiate<Bakeneko>(catPrefab, spawnCatPos.position, Quaternion.identity);
        CatTransform = neko.transform;
    }

    public void GameOver()
    {
        Debug.Log("game over");
    }
}
