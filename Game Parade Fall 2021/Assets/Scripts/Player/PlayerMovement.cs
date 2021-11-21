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
    float currentStamina = 0f;
    [SerializeField]
    float staminaCostRate = 0.01f;
    [SerializeField]
    float staminaRegenerateRate = 0.02f;

    Vector2 movementInput;
    float speed = 0f;
    bool isSprintButtonHeld = false;

    new Rigidbody rigidbody;

    public bool _restrictMovement = false;

    public bool IsSprinting => isSprinting;
    public float CurrentStamina => currentStamina;

    public Vector3 Direction { get; private set; }
    public float Speed => speed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = walkSpeed;
        currentStamina = 100f;

        InputManager.Actions.Player.Sprint.started += OnSprintButtonStart; 
        InputManager.Actions.Player.Sprint.canceled += OnSprintButtonCancel;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Pause.onPause += FreeCursor;
        GameManager.Instance.onGameOver += FreeCursorOnGameOver;
        GameManager.Instance.onBirdWon += FreeCursorOnGameOver;
        GameManager.Instance.onBirdCaught += RestrictMovement;
        GameManager.Instance.onBirdReset += AllowMovement;
    }

    public void RestrictMovement() { _restrictMovement = true; }
    public void AllowMovement() { _restrictMovement = false; }

    void FreeCursorOnGameOver()
    {
        FreeCursor(true);
        GameManager.Instance.onGameOver -= FreeCursorOnGameOver;
        GameManager.Instance.onBirdWon -= FreeCursorOnGameOver;
        GameManager.Instance.onBirdCaught -= RestrictMovement;
        GameManager.Instance.onBirdReset -= AllowMovement;
    }

    private void FreeCursor(bool free)
    {
        if (free)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (_restrictMovement)
            return;
        ApplyMovement();
    }

    void GetInput ()
    {
        movementInput = InputManager.Actions.Player.Movement.ReadValue<Vector2>();
    }

    void ApplyMovement ()
    {
        var moveRight = transform.right * movementInput.x;
        var moveForward = transform.forward * movementInput.y;
        Direction = moveRight + moveForward;

        rigidbody.MovePosition(rigidbody.position + Direction*speed);
        
        if (isSprintButtonHeld && currentStamina > 0f)
        {
            speed = sprintSpeed;
            isSprinting = true;
        } 
        else
        {
            speed = walkSpeed;
            isSprinting = false;
        }
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
        if (currentStamina < 100f)
            currentStamina += staminaRegenerateRate;
        else
            currentStamina = 100f;
    }

    void OnSprintButtonStart (InputAction.CallbackContext ctx) 
    {
        isSprintButtonHeld = true;
        InvokeRepeating("DepleteStamina", 0f, 0.1f); 
        CancelInvoke("RegenerateStamina");
    }

    void OnSprintButtonCancel (InputAction.CallbackContext ctx)
    {
        isSprintButtonHeld = false;
        InvokeRepeating("RegenerateStamina", 0f, 0.1f); 
        CancelInvoke("DepleteStamina");
    }
}
