using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    float initialDelay = 1f;
    [SerializeField]
    GameObject movementTutorial;
    [SerializeField]
    GameObject sprintTutorial;
    [SerializeField]
    GameObject markTutorial;
    [SerializeField]
    GameObject finishTutorial;

    private void Start()
    {
        InputManager.Actions.Player.Movement.Disable();
        InputManager.Actions.Player.Sprint.Disable();
        InputManager.Actions.Player.Mark.Disable();

        Invoke("Movement", initialDelay);
    }

    void Movement ()
    {
        movementTutorial.SetActive(true);
        InputManager.Actions.Player.Movement.Enable();
        InputManager.Actions.Player.Movement.performed += Sprint;
    }

    void Sprint (InputAction.CallbackContext ctx)
    {
        sprintTutorial.SetActive(true); movementTutorial.SetActive(false);
        InputManager.Actions.Player.Sprint.Enable();
        InputManager.Actions.Player.Sprint.performed += Mark;
        InputManager.Actions.Player.Movement.performed -= Sprint;
    }

    void Mark (InputAction.CallbackContext ctx)
    {
        markTutorial.SetActive(true); sprintTutorial.SetActive(false);
        InputManager.Actions.Player.Mark.Enable();
        InputManager.Actions.Player.Mark.performed += FinishTutorial;
        InputManager.Actions.Player.Sprint.performed -= Mark;
    }

    void FinishTutorial (InputAction.CallbackContext ctx)
    {
        finishTutorial.SetActive(true); markTutorial.SetActive(false);
        StartCoroutine(DisableTutorial());
        InputManager.Actions.Player.Mark.performed -= FinishTutorial;
    }

    IEnumerator DisableTutorial ()
    {
        yield return new WaitForSeconds(3f);
        finishTutorial.SetActive(false);
    }
}
