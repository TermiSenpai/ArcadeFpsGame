using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovConfig config;
    public Transform orientation;

    private Vector2 movementInput;

    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        rb.AddForce(moveDirection.normalized * config.moveSpeed * 10f, ForceMode.Force);
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
}
