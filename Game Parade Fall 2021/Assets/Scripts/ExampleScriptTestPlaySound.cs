using UnityEngine;

public class ExampleScriptTestPlaySound:MonoBehaviour
{
    [SerializeField]
    string soundName;

    [ContextMenu("PlaySound")]
    void PlaySound()
    {
        EventManager.Instance.TriggerEvent("PlaySound", soundName);
    }
}
