using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    bool isSprinting = false;
    [SerializeField]
    float walkSpeed = 5f;
    [SerializeField]
    float sprintSpeed = 10f;
    [SerializeField]
    float maxStamina = 100f;
    [SerializeField]
    float currentStamina = 0f;
    [SerializeField]
    float staminaCostRate = 0.01f;
    [SerializeField]
    float staminaRegenerateRate = 0.02f;

    Vector2 movementInput;
    float speed = 0f;

    new Rigidbody rigidbody;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = walkSpeed;
        currentStamina = maxStamina;

        InputManager.Actions.Player.Sprint.started += OnSprintStart; 
        InputManager.Actions.Player.Sprint.canceled += OnSprintEnd;

       Cursor.visible = false;
       Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.onGameOver += FreeCursorOnGameOver;
    }

    void FreeCursorOnGameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.onGameOver -= FreeCursorOnGameOver;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    void GetInput ()
    {
        movementInput = InputManager.Actions.Player.Movement.ReadValue<Vector2>();
    }

    void ApplyMovement ()
    {
        var moveRight = transform.right * movementInput.x * speed;
        var moveForward = transform.forward * movementInput.y * speed;
        rigidbody.MovePosition(rigidbody.position + moveRight + moveForward);

        speed = isSprinting && currentStamina > 0f ? sprintSpeed : walkSpeed;
    }

    

    void DepleteStamina ()
    {
        if (currentStamina > 0f)
            currentStamina -= staminaCostRate;
        else
            currentStamina = 0f;
    }

    void RegenerateStamina ()
    {
        if (currentStamina < maxStamina)
            currentStamina += staminaRegenerateRate;
        else
            currentStamina = maxStamina;
    }

    void OnSprintStart (InputAction.CallbackContext ctx) 
    {
        isSprinting = true;
        InvokeRepeating("DepleteStamina", 0f, 0.1f); 
        CancelInvoke("RegenerateStamina");
    }

    void OnSprintEnd (InputAction.CallbackContext ctx)
    {
        isSprinting = false;
        InvokeRepeating("RegenerateStamina", 0f, 0.1f); 
        CancelInvoke("DepleteStamina");
    }
}
