using System;
using UnityEngine;

public class PausePanel : ToggleablePanel
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _settingsMenu;
    private void Start()
    {
        Pause.onPause += TogglePause;
    }

    private void OnDestroy()
    {
        Pause.onPause -= TogglePause;
    }

    private void TogglePause(bool pause)
    {
        if (pause)
        { 
            _pauseMenu.SetActive(true);
            _settingsMenu.SetActive(false);
            Show();
        }
        else
        {
            Hide();
        }
    }
}