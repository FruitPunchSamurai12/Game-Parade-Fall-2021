using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ToggleablePanel : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    public bool IsVisible => _canvasGroup.alpha > 0;
    public bool _hideOnStart = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if(_hideOnStart)
            Hide();
    }


    public void ToggleState()
    {
        if (IsVisible)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}