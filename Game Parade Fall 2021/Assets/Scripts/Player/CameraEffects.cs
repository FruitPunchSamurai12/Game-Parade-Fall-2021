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

    private void OnEnable()
    {
        GameManager.Instance.onBirdCaught += ResetVignette;
    }

    private void OnDisable()
    {
        GameManager.Instance.onBirdCaught -= ResetVignette;
    }

    private void Start()
    {
        volume.profile.TryGet(out vignette);
    }

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
            vignette.smoothness.value = (vignetteStartDistance - distance) * vignetteTakeoverSpeed;
        }
    }

    void ResetVignette ()
    {
        vignette.smoothness.value = 0.01f;
    }
}
