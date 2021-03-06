using System;
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

    Vignette vignette;

    public static event Action onCatWithinRange;
    public static event Action onCatOutOfRange;

    private void OnEnable()
    {
        GameManager.Instance.onBirdReset += ResetVignette;
    }

    private void OnDisable()
    {
        GameManager.Instance.onBirdReset -= ResetVignette;
    }

    private void Awake ()
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
            onCatWithinRange?.Invoke();
        }
        else
        {
            onCatOutOfRange?.Invoke();
        }
    }

    void ResetVignette ()
    {
        vignette.smoothness.value = 0.01f;
    }
}
