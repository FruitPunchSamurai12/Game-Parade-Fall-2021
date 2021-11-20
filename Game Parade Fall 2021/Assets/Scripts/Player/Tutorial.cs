using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    float initialDelay = 1f;
    [SerializeField]
    string movementText = "USE [W][A][S][D] TO MOVE";
    [SerializeField]
    string sprintText = "HOLD [SHIFT] WHILE MOVING TO SPRINT \n WATCH OUT YOUR STAMINA GAUGE";
    [SerializeField]
    string markText = "PRESS [E] TO PLACE FEATHERS \n USE FEATHERS TO FIND EXIT";
    [SerializeField]
    string finishText = "BEWARE THE CAT! AND GOOD LUCK!";

    TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InputManager.Actions.Player.Movement.Disable();
        InputManager.Actions.Player.Sprint.Disable();
        InputManager.Actions.Player.Mark.Disable();

        Invoke("Movement", initialDelay);
    }

    void Movement ()
    {
        textComponent.text = movementText;
        InputManager.Actions.Player.Movement.Enable();
        InputManager.Actions.Player.Movement.performed += Sprint;
    }

    void Sprint (InputAction.CallbackContext ctx)
    {
        textComponent.text = sprintText;
        InputManager.Actions.Player.Sprint.Enable();
        InputManager.Actions.Player.Sprint.performed += Mark;
        InputManager.Actions.Player.Movement.performed -= Sprint;
    }

    void Mark (InputAction.CallbackContext ctx)
    {
        textComponent.text = markText;
        InputManager.Actions.Player.Mark.Enable();
        InputManager.Actions.Player.Mark.performed += FinishTutorial;
        InputManager.Actions.Player.Sprint.performed -= Mark;
    }

    void FinishTutorial (InputAction.CallbackContext ctx)
    {
        textComponent.text = finishText;
        InputManager.Actions.Global.AnyKey.performed += DisableTutorial;
        InputManager.Actions.Player.Mark.performed -= FinishTutorial;
    }

    void DisableTutorial (InputAction.CallbackContext ctx)
    {
        textComponent.text = "";
        InputManager.Actions.Global.AnyKey.performed -= DisableTutorial;
    }
}
