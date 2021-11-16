using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject cameraPrefab;

    [Header("Movement")]
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

    [Header("Rotation")]
    [SerializeField]
    float sensivity = 0.5f;
    [SerializeField]
    float xLimit = 45f;
    [SerializeField] [Range(0f, 1f)]
    float playerRotationSmoothness = 0.5f;

    Vector2 movementInput;
    Vector2 mouseInput;
    float speed = 0f;
    float yRot = 0f;
    float xRot = 0f;

    new Rigidbody rigidbody;
    Transform cameraTarget;

    private void OnEnable()
    {
        InputManager.Actions.Player.Sprint.started += ctx => isSprinting = true;
        InputManager.Actions.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        speed = walkSpeed;
        currentStamina = maxStamina;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Instantiate(cameraPrefab, transform.position, Quaternion.identity);
        cameraTarget = GameObject.FindWithTag("CameraTarget").transform;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();

        Stamina();
    }

    void GetInput ()
    {
        movementInput = InputManager.Actions.Player.Movement.ReadValue<Vector2>();
        mouseInput = InputManager.Actions.Player.CameraRotation.ReadValue<Vector2>();
    }

    void ApplyMovement ()
    {
        var moveRight = transform.right * movementInput.x * speed;
        var moveForward = transform.forward * movementInput.y * speed;
        rigidbody.MovePosition(rigidbody.position + moveRight + moveForward);

        speed = isSprinting && currentStamina > 0f ? sprintSpeed : walkSpeed;
    }

    void Stamina ()
    {
        if (isSprinting && currentStamina > 0f && movementInput != Vector2.zero)
            currentStamina -= staminaCostRate;
        else if (!isSprinting && currentStamina < maxStamina)
            currentStamina += staminaRegenerateRate;

        if (currentStamina > maxStamina) currentStamina = maxStamina;
        else if (currentStamina < 0f) currentStamina = 0f;
    }

    void ApplyRotation ()
    {
        mouseInput *= sensivity;

        yRot += mouseInput.x;

        xRot += mouseInput.y;
        xRot = Mathf.Clamp(xRot, -xLimit, xLimit);

        cameraTarget.localEulerAngles = new Vector3(xRot, yRot, 0f);

        // Make player face camera forward direction while moving
        var playerRotation = new Vector3(0f, cameraTarget.transform.rotation.eulerAngles.y, 0f);
        if (movementInput != Vector2.zero) 
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, playerRotation, 1 - playerRotationSmoothness));
    }
}
