using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMarkerPlacer : MonoBehaviour
{
    [SerializeField]
    PooledMonoBehaviour mark;
    [SerializeField]
    float markingDelay = 2f;
    [SerializeField]
    Vector3 markPositionOffset;

    private void OnEnable()
    {
        InputManager.Actions.Player.Mark.performed += OnMarkPressed;
    }

    private void OnDisable()
    {
        InputManager.Actions.Player.Mark.performed -= OnMarkPressed;
    }

    void OnMarkPressed (InputAction.CallbackContext ctx)
    {
        StartCoroutine(Mark());
    }

    IEnumerator Mark ()
    {
        InputManager.Actions.Player.Disable();

        yield return new WaitForSeconds(markingDelay);


        var newMark = mark.Get<PooledMonoBehaviour>(transform.position + markPositionOffset, Quaternion.identity);
        Director.Instance.NewMarkedPlaced(transform.position + markPositionOffset);
        //newMark.transform.localPosition += markPositionOffset;
        //newMark.transform.parent = null;

        InputManager.Actions.Player.Enable();
    }
}
