using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class ToggleablePanel : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    public bool IsVisible => _canvasGroup.alpha > 0;
    public bool _hideOnStart = false;

    public float _showTime;
    public float _hideTime;

    float _timer = 0;
    [SerializeField] AnimationCurve _showCurve;
    [SerializeField] AnimationCurve _hideCurve;
    [SerializeField] GameObject _objectToToggleActive;

    public event Action onShowComplete;
    public event Action onHideComplete;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_hideOnStart)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }


    public void ToggleState()
    {  
        if (IsVisible)
            Hide();
        else
            Show();
    }
    [ContextMenu("Show")]
    public void Show()
    {

        _timer = 0;
        StopAllCoroutines();
        if (_objectToToggleActive != null)
            _objectToToggleActive.SetActive(true);
        StartCoroutine(ShowCoroutine());
    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        _timer = 0;
        StopAllCoroutines();       
        StartCoroutine(HideCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        while(_timer<_showTime)
        {
            _timer+=Time.unscaledDeltaTime;
            float percentage = _timer / _showTime;
            _canvasGroup.alpha = _showCurve.Evaluate(percentage);
            yield return null;
        }
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        
        onShowComplete?.Invoke();
    }

    IEnumerator HideCoroutine()
    {
        while (_timer < _hideTime)
        {
            _timer += Time.deltaTime;
            float percentage = _timer / _hideTime;
            _canvasGroup.alpha =1- _hideCurve.Evaluate(percentage);
            yield return null;
        }
        if (_objectToToggleActive != null)
            _objectToToggleActive.SetActive(false);
        _canvasGroup.alpha =  0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        onHideComplete?.Invoke();
    }

}
