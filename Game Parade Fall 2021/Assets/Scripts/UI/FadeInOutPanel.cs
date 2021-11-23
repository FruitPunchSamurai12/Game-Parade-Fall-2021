using System;

public class FadeInOutPanel:ToggleablePanel
{
    private void Start()
    {
        GameStateMachine.Instance.onLoadingComplete += HandleLoadingComplete;
    }

    void HandleLoadingComplete()
    {
        GameStateMachine.Instance.onLoadingComplete -= HandleLoadingComplete;
        GameManager.Instance.onBirdCaught += Show;
        onShowComplete += HandleShowComplete;
        Hide();
    }

    private void HandleShowComplete()
    {
        GameManager.Instance.ResetBird();
        Hide();
    }
}