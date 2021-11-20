using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance CatDial;
    private FMOD.Studio.EventInstance CatFootsteps;

    private void OnEnable()
    {
        CatAnimator.onStep += PlayStepSound;
    }

    private void OnDisable()
    {
        CatAnimator.onStep -= PlayStepSound;
    }

    // Start is called before the first frame update
    void Start()
    {
        CatDial = FMODUnity.RuntimeManager.CreateInstance("event:/CatDial");
        CatDial.start();
    }

    // Update is called once per frame
    void Update()
    {
        CatDial.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        //CatFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    void PlayStepSound ()
    {
        //CatFootsteps = FMODUnity.RuntimeManager.CreateInstance("event:/CatFootsteps");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(CatDial, GetComponent<Transform>(), GetComponent<Rigidbody>());
        //CatFootsteps.start();
    }
}
