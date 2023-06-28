using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig movementConfig;
    [SerializeField] PlayerJump playerJump;
    [SerializeField] Transform orientation;
    Rigidbody rb;

    Vector2 movementInput;
    Vector3 playerDir;

    PhotonView pv;
    RaycastHit slopeHit;
    float playerHeight = 2f;
    Vector3 slopeMoveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!pv.IsMine)
            Destroy(rb);
        rb.freezeRotation = true;
        playerHeight = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        Movementent();
        controlDrag();
    }

    private void Update()
    {
        slopeMoveDirection = Vector3.ProjectOnPlane(playerDir, slopeHit.normal);
    }

    private void Movementent()
    {

        playerDir = orientation.forward * movementInput.y * movementConfig.fowardSpeed + orientation.right * movementInput.x * movementConfig.strafeSpeed;

        // IsGrounded and not onSlope
        if (playerJump.isGrounded() && !onSlope())
            rb.AddForce(playerDir.normalized * movementConfig.movementMultiplier, ForceMode.Acceleration);

        // Is jumping
        else if (!playerJump.isGrounded())
            rb.AddForce(playerDir.normalized * movementConfig.airMultiplier * movementConfig.airMultiplier, ForceMode.Acceleration);

        // IsGrounded and onSlope
        else if (playerJump.isGrounded() && onSlope())
            rb.AddForce(slopeMoveDirection.normalized * movementConfig.movementMultiplier, ForceMode.Acceleration);

    }

    private void controlDrag()
    {
        if (playerJump.isGrounded())
            rb.drag = movementConfig.groundDrag;

        else if(!playerJump.isGrounded())
            rb.drag = movementConfig.airDrag;
            
    }

    bool onSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
                return true;
            return false;
        }
        return false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (pv.IsMine)
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    movementInput = context.ReadValue<Vector2>();
                    break;

                case InputActionPhase.Canceled:
                    movementInput = Vector2.zero;
                    break;
            }
    }
}
