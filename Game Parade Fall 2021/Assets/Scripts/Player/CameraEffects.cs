using System.Collections; 
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffects : MonoBehaviour
{
    [Header("Vignette")]
    [SerializeField]
    Volume volume;
    [SerializeField]
    float vignetteStartDistance = 10f;
    [SerializeField]
    float vignetteTakeoverSpeed = 0.1f;

    [Header("Camera Shake")]
    [SerializeField]
    float shakeMagnitude = 1f;
    [SerializeField]
    float shakeDuration = 1f;
    [SerializeField]
    float shakeStartDistance = 30f;
    [SerializeField]
    float shakeIncreaseStrength = 0.005f;

    Vignette vignette;

    private void OnEnable()
    {
        GameManager.Instance.onBirdReset += ResetVignette;
    }

    private void OnDisable()
    {
        GameManager.Instance.onBirdReset -= ResetVignette;
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
