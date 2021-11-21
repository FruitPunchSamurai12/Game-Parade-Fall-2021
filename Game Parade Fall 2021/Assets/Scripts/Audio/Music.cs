using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private FMOD.Studio.EventInstance music;
    private FMOD.Studio.EventInstance ambient;
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
