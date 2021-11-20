using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    bool invertedXRot = false;
    [SerializeField]
    float sensivityH = 0.5f;
    [SerializeField]
    float sensiviryV = 0.5f;
    [SerializeField]
    float xLimit = 45f;
    [SerializeField]
    [Range(0f, 1f)]
    float playerRotationSmoothness = 0.5f;

    Vector2 mouseInput;
    Vector2 movementInput;
    float yRot = 0f;
    float xRot = 0f;

    Transform player;

    private void Start()
    {
        player = GameManager.Instance.PlayerTransform;
    }

    private void Update()
    {
        GetInput();
        ApplyRotation();
    }

    void GetInput ()
    {
        mouseInput = InputManager.Actions.Camera.Rotation.ReadValue<Vector2>();
        movementInput = InputManager.Actions.Player.Movement.ReadValue<Vector2>();
    }

    void ApplyRotation()
    {
        mouseInput = new Vector2(mouseInput.x * sensivityH, mouseInput.y * sensiviryV);

        yRot += mouseInput.x;

        xRot += mouseInput.y;
        xRot = Mathf.Clamp(xRot, -xLimit, xLimit);

        var fixedXRot = invertedXRot ? xRot : -xRot;

        transform.localEulerAngles = new Vector3(fixedXRot, yRot, 0f);

        // Make player face camera forward direction while moving
        var playerRotationEuler = new Vector3(0f, transform.transform.rotation.eulerAngles.y, 0f);
        var playerRotation = Quaternion.Euler(playerRotationEuler);
        if (movementInput != Vector2.zero)
            player.rotation = Quaternion.Lerp(player.rotation, playerRotation, 1f - playerRotationSmoothness);
    }
}
