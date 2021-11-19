using System;

public class FadeInOutPanel:ToggleablePanel
{
    private void Start()
    {
        GameManager.Instance.onBirdCaught += Show;
        onShowComplete += HandleShowComplete;
    }

    private void HandleShowComplete()
    {
        GameManager.Instance.ResetBird();
        Hide();
    }
}