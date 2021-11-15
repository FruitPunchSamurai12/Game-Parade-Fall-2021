using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputActions Actions { get; private set; }

    private void OnEnable()
    {
        if (Actions == null)
        {
            Actions = new InputActions();
            Actions.Enable();
        } else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        Actions.Disable();
    }
}
