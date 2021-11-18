using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : ToggleablePanel
{
    private void Start()
    {
        GameManager.Instance.onGameOver += HandleGameOver;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameOver -= HandleGameOver;
    }

    void HandleGameOver()
    {
        Show();
    }

}
