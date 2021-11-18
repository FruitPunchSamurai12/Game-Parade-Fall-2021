using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputActions Actions { get; private set; }

    private void OnEnable()
    {
        Actions = new InputActions();
        Actions.Enable();

    }

    private void OnDisable()
    {
        Actions.Disable();
    }
}
