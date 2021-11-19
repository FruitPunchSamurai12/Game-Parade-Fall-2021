using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance CatDial;
    // Start is called before the first frame update
    void Start()
    {
        CatDial = FMODUnity.RuntimeManager.CreateInstance("event:/CatDial");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(CatDial, GetComponent<Transform>(), GetComponent<Rigidbody>());
        CatDial.start();
    }

    // Update is called once per frame
    void Update()
    {
        CatDial.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }
}
