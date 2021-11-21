using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private FMOD.Studio.EventInstance music;
    private FMOD.Studio.EventInstance ambient;

    float musicUserVolume = GameSettings.Instance.music;
    float sfxUserVolume = GameSettings.Instance.sfx;

    // Start is called before the first frame update
    void Start()
    {
        music = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        music.start();
        ambient = FMODUnity.RuntimeManager.CreateInstance("event:/Ambient");
        ambient.start();
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GameState", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float musicUserVolume = GameSettings.Instance.music;
        float sfxUserVolume = GameSettings.Instance.sfx;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("musicUserVolume", musicUserVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("sfxUserVolume", sfxUserVolume);
    }
    private void OnEnable()
    {
        GameManager.Instance.onBirdCaught += CatchedState;
        GameManager.Instance.onGameOver += GameOverSting;
        GameManager.Instance.onBirdReset += InitialState;
    }
    private void OnDisable()
    {
        GameManager.Instance.onBirdCaught -= CatchedState;
        GameManager.Instance.onGameOver -= GameOverSting;
    }
    void CatchedState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GameState", 1f);
        Debug.Log("catched");
    }
    void GameOverSting()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GameState", 2f);
    }
    void InitialState()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GameState", 0f);
    }
}
