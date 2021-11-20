using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMarkerPlacer : MonoBehaviour
{
    [SerializeField]
    bool canMark = true;
    [SerializeField]
    PooledMonoBehaviour mark;
    [SerializeField]
    float markingDelay = 2f;
    [SerializeField]
    float delayBetweenMarks = 2f;
    [SerializeField]
    Transform placementTransform;

    public bool CanMark => canMark;

    private void OnEnable()
    {
        InputManager.Actions.Player.Mark.performed += OnMarkPressed;
        PlayerAnimator.onBlow += OnMark;
    }

    private void OnDisable()
    {
        InputManager.Actions.Player.Mark.performed -= OnMarkPressed;
        PlayerAnimator.onBlow -= OnMark;
    }

    void OnMarkPressed (InputAction.CallbackContext ctx)
    {
        if (canMark)
            StartCoroutine(WaitToMark());
    }

    void OnMark ()
    {
        var newMark = mark.Get<PooledMonoBehaviour>(false);
        newMark.transform.position = placementTransform.position;
        newMark.gameObject.SetActive(true);
        Director.Instance.NewMarkedPlaced(newMark.transform.position);
        StartCoroutine(DisableMark());
    }

    IEnumerator WaitToMark ()
    {
        GetComponent<PlayerMovement>().RestrictMovement();
        InputManager.Actions.Player.Mark.Disable();
        yield return new WaitForSeconds(markingDelay);
        GetComponent<PlayerMovement>().AllowMovement();
        InputManager.Actions.Player.Mark.Enable();
    }

    IEnumerator DisableMark ()
    {
        canMark = false;
        yield return new WaitForSeconds(delayBetweenMarks);
        canMark = true;
    }
}
