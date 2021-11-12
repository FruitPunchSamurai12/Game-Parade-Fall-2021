using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlewareTest : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.StartListening("PlaySound", DebugPrint);
    }

    private void OnDestroy()
    {
        EventManager.Instance.StopListening("PlaySound", DebugPrint);
    }

    public void DebugPrint(object soundName)
    {
        string soundNameString = soundName.ToString();
        Debug.Log($"Playing song {soundNameString}");
    }
}
