using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 0.9f)]
    float transitionSmoothness = 0.5f;

    Animator animator;
    PlayerMovement movement;
    PlayerMarkerPlacer markerPlacer;

    float xMove = 0f;
    float yMove = 0f;

    public static event Action onStep;
    public static event Action onMarkingStart;
    public static event Action onBlow;

    private void OnEnable()
    {
        InputManager.Actions.Player.Mark.performed += OnMarkPressed;
    }

    private void OnDisable()
    {
        InputManager.Actions.Player.Mark.performed -= OnMarkPressed;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        markerPlacer = GetComponentInParent<PlayerMarkerPlacer>();
    }

    private void Update()
    {
        var movementInput = InputManager.Actions.Player.Movement.ReadValue<Vector2>();

        xMove = Mathf.Lerp(xMove, movementInput.x, 1f - transitionSmoothness);
        yMove = Mathf.Lerp(yMove, movement.IsSprinting ? movementInput.y * 2f : movementInput.y, 1f - transitionSmoothness);

        animator.SetFloat("xMove", xMove);
        animator.SetFloat("yMove", yMove);
    }

    void OnMarkPressed(InputAction.CallbackContext ctx)
    {
        if (markerPlacer.CanMark)
        {
            animator.SetTrigger("Mark");
            onMarkingStart?.Invoke();
        }
    }

    public void Blow () => onBlow?.Invoke();
    public void Footstep () => onStep?.Invoke();
}
