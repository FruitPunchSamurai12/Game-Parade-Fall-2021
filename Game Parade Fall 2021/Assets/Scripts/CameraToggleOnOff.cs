using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggleOnOff : MonoBehaviour
{
    Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _cam.enabled = false;
    }

    void Start()
    {
        GameStateMachine.Instance.onLoadingComplete += HandleLevelLoaded;
    }

    void HandleLevelLoaded()
    {
        _cam.enabled = true;
        GameStateMachine.Instance.onLoadingComplete += HandleLevelLoaded;
    }
}
