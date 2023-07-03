using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig config;
    PlayerController playerController;
    public Transform orientation;

    [Header("Inputs and movement")]
    private Vector2 movementInput;
    PhotonView pv;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Slopes")]
    [SerializeField] private RaycastHit slopeHit;
    const float playerHeight = 5;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if(!pv.IsMine)        
            Destroy(rb);
        
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        controlDrag();
        SpeedControl();
    }

    private void controlDrag()
    {
        if (playerController.IsGrounded())        
            rb.drag = config.groundDrag;
        
        else
            rb.drag = 0;

    }

    private void FixedUpdate()
    {
        if(!pv.IsMine) return;

        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        //On slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDir() * config.moveSpeed * config.movMultiplier, ForceMode.Force);
        }

        //On ground
        else if(playerController.IsGrounded())
            rb.AddForce(moveDirection.normalized * config.moveSpeed * config.movMultiplier, ForceMode.Force);

        // On air
        else if(!playerController.IsGrounded())
            rb.AddForce(moveDirection.normalized * config.moveSpeed * config.movMultiplier * config.airMovMultiplier, ForceMode.Force);


    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
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

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //limit velocity if needed
        if(flatVel.magnitude > config.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * config.moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < config.maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, Vector3.down);
    }

    private Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
