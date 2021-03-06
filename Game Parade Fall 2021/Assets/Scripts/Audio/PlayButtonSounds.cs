using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class PlayButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] string _soundName;

    public void OnPointerClick(PointerEventData eventData)
    {
        //PlayClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHover();
    }

    void PlayClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIGameLaunch");
        Debug.Log("Click");
    }
    void PlayHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIMouseOver");
        Debug.Log("Hover");
    }

}