using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] PlayerIngameSettings settings;
    [SerializeField] PlayerMovConfig config;

    CharacterController controller;
    PlayerJump player;

    float curMovementSpeed;
    Vector2 movementInput;
    Vector3 moveDirection;
    Vector3 currentSpeed;
    Vector3 velocityToSet;

    public bool isFreeze = false;
    public bool isActiveGrapple = false;
    
    PhotonView pv;
    #endregion

    #region Unity
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
        ControlSpeed();
        FreezePlayer();
    }
    #endregion

    #region custom

    private void ControlSpeed()
    {
        if (player.IsGrounded())
        {
            curMovementSpeed = config.groundSpeed;
        }

        else if (!player.IsGrounded())
        {
            curMovementSpeed = config.airSpeed;
        }
    }

    private void IncreaseSpeed()
    {
        // Calcular la velocidad objetivo
        Vector3 objetiveSpeed = moveDirection * config.maxSpeed;

        // Calcular la aceleración
        currentSpeed = Vector3.Lerp(currentSpeed, objetiveSpeed, config.acceleration * Time.deltaTime);
    }

    private void ReduceSpeed()
    {
        switch (player.IsGrounded())
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

    private void SetVelocity() => currentSpeed = velocityToSet;

    private void MovePlayer()
    {
        // Normalized in PlayerInputs Processor
        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        // Si hay movimiento
        if (moveDirection.magnitude >= 0.1f)
        {
            IncreaseSpeed();
        }
        else
            ReduceSpeed();

        controller.Move(curMovementSpeed * Time.deltaTime * currentSpeed);
    }

    public Vector3 GetMovementDir()
    {
        return moveDirection;
    }

    public void JumpToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        isActiveGrapple = true;
        velocityToSet = player.CalculateJumpVelocity(transform.position, targetPos, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private void FreezePlayer()
    {
        if (!isFreeze) return;

        currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.grapplingDeceleration * Time.deltaTime);

    }

    #endregion

    #region input
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        if (settings.GetState() == State.paused)
        {
            movementInput = Vector2.zero;
            return;
        }

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
    #endregion
}

