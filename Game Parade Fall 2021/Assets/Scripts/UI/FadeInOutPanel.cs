using System;
using UnityEngine;

public class FadeInOutPanel:ToggleablePanel
{
    private void Start()
    {
        Debug.Log("fade panel start");
        GameStateMachine.Instance.onLoadingComplete += HandleLoadingComplete;
    }

    void HandleLoadingComplete()
    {
        Debug.Log("fade panel handle loading complete");
        GameStateMachine.Instance.onLoadingComplete -= HandleLoadingComplete;
        GameManager.Instance.onBirdCaught += Show;
        onShowComplete += HandleShowComplete;
        Hide();
    }

    private void HandleShowComplete()
    {
        Debug.Log("fade panel handle show complete");
        GameManager.Instance.ResetBird();
        Hide();
    }
}