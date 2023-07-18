using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    [SerializeField] PlayerMovConfig config;
    CharacterController controller;
    PlayerJump player;

    float curMovementSpeed;

    Vector2 movementInput;
    Vector3 moveDirection;
    Vector3 currentSpeed;
    public bool isFreeze = false;
    public bool isActiveGrapple = false;
    PhotonView pv;
    Vector3 velocityToSet;

    private void Awake()
    {
        player = GetComponent<PlayerJump>();
        pv = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!pv.IsMine) return;
        if (isActiveGrapple) return;

        MovePlayer();
        controlSpeed();
        FreezePlayer();
    }

    private void controlSpeed()
    {
        if (player.isGrounded())
        {
            curMovementSpeed = config.groundSpeed;
        }

        else if (!player.isGrounded())
        {
            curMovementSpeed = config.airSpeed;
        }
    }

    private void MovePlayer()
    {
        // Normalized in PlayerInputs Processor
        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        // Si hay movimiento
        if (moveDirection.magnitude >= 0.1f)
        {
            increaseSpeed();
        }
        else
            reduceSpeed();

        controller.Move(currentSpeed * curMovementSpeed * Time.deltaTime);
    }

    public void JumpToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        isActiveGrapple = true;
        velocityToSet = player.CalculateJumpVelocity(transform.position, targetPos, trajectoryHeight);
        Invoke(nameof(setVelocity), 0.1f);
    }

    private void setVelocity()
    {
        currentSpeed = velocityToSet;
    }

    private void increaseSpeed()
    {
        // Calcular la velocidad objetivo
        Vector3 objetiveSpeed = moveDirection * config.maxSpeed;

        // Calcular la aceleración
        currentSpeed = Vector3.Lerp(currentSpeed, objetiveSpeed, config.acceleration * Time.deltaTime);
    }

    private void reduceSpeed()
    {
        switch (player.isGrounded())
        {
            case true:
                // Si no hay movimiento, detener al personaje
                currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.groundDeceleration * Time.deltaTime);
                break;
            case false:
                currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.airDeceleration * Time.deltaTime);
                break;
        }
    }

    private void FreezePlayer()
    {
        if (!isFreeze) return;

        currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.grapplingDeceleration * Time.deltaTime);

    }

    public Vector3 getMovementDir()
    {
        return moveDirection;
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                movementInput = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Performed:
                movementInput = context.ReadValue<Vector2>();
                break;

            case InputActionPhase.Canceled:
                movementInput = Vector2.zero;
                break;
        }
    }
}

