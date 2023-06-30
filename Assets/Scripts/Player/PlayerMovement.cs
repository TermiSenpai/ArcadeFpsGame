using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig config;
    [SerializeField] PlayerJump playerJump;
    public Transform orientation;

    private Vector2 movementInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (playerJump.IsGrounded())
        {
            rb.drag = config.groundDrag;
        }
        else
            rb.drag = 0;

        SpeedControl();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        if(playerJump.IsGrounded())
            rb.AddForce(moveDirection.normalized * config.moveSpeed * config.movMultiplier, ForceMode.Force);
        else if(!playerJump.IsGrounded())
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
}
