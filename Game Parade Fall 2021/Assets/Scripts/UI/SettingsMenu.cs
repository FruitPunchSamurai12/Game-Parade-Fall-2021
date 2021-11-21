using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    Slider sfxSlider;
    [SerializeField]
    Slider musicSlider;

    private void Start()
    {
        sfxSlider.value = GameSettings.Instance.sfx;
        musicSlider.value = GameSettings.Instance.music;
    }

    public void OnSFXChanged(float value) => GameSettings.Instance.sfx = value;
    public void OnMusicChanged(float value) => GameSettings.Instance.music = value;
}
