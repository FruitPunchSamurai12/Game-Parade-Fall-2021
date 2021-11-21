using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] string _soundName;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHover();
    }

    void PlayClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIClick");
        Debug.Log("Click");
    }
    void PlayHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIMouseOver");
        Debug.Log("Hover");
    }

}