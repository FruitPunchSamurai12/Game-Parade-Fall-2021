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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
