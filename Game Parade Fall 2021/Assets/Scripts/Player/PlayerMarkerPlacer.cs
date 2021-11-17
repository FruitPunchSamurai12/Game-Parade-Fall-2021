using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMarkerPlacer : MonoBehaviour
{
    [SerializeField]
    GameObject mark;
    [SerializeField]
    float markingDelay = 2f;
    [SerializeField]
    Vector3 markPositionOffset;

    private void OnEnable()
    {
        InputManager.Actions.Player.Mark.performed += OnMarkPressed;
    }

    void OnMarkPressed (InputAction.CallbackContext ctx)
    {
        StartCoroutine(Mark());
    }

    IEnumerator Mark ()
    {
        InputManager.Actions.Player.Disable();

        yield return new WaitForSeconds(markingDelay);

        var newMark = Instantiate(mark, transform);
        newMark.transform.localPosition += markPositionOffset;
        newMark.transform.parent = null;

        InputManager.Actions.Player.Enable();
    }
}
