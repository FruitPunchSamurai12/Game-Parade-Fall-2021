using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance OwlFootsteps;
    private FMOD.Studio.EventInstance FeatherDrop;

    private void OnEnable()
    {
        PlayerAnimator.onStep += PlayStepSound;
        PlayerAnimator.onMarkingStart += PlayFeatherDropSound;
    }

    private void OnDisable()
    {
        PlayerAnimator.onStep -= PlayStepSound;
        PlayerAnimator.onMarkingStart -= PlayFeatherDropSound;
    }
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        OwlFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FeatherDrop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }

    void PlayFeatherDropSound()
    {
      FeatherDrop = FMODUnity.RuntimeManager.CreateInstance("event:/FeatherDrop");
      FeatherDrop.start();
    }
    void PlayStepSound()
    {
        OwlFootsteps = FMODUnity.RuntimeManager.CreateInstance("event:/OwlFootsteps");
        OwlFootsteps.start();
    }
}