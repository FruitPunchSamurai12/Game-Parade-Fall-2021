using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    Volume volume;
    [SerializeField]
    float vignetteStartDistance = 10f;
    [SerializeField]
    float vignetteTakeoverSpeed = 0.1f;

    Vignette vignette;

    private void Update()
    {
        DangerVignette();
    }

    void DangerVignette ()
    {
        var distance = Vector3.Distance(GameManager.Instance.PlayerTransform.position, GameManager.Instance.CatTransform.position);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CatDistance", distance);
        if (distance < vignetteStartDistance)
        {
            volume.profile.TryGet(out vignette);
            vignette.smoothness.value = (vignetteStartDistance - distance) * vignetteTakeoverSpeed;
        }
    }
}
