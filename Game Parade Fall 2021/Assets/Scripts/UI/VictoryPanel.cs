public class VictoryPanel : ToggleablePanel
{
    private void Start()
    {
        GameManager.Instance.onBirdWon += HandleVictory;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onBirdWon -= HandleVictory;
    }

    void HandleVictory()
    {
        Show();
    }

}
