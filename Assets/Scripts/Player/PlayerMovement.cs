using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig movementConfig;
    [SerializeField] Rigidbody rb;

    Vector2 movementInput;
    Vector3 moveAmount;
    Vector3 smoothMoveSpeed;
    float smoothTime;


    private void FixedUpdate()
    {
        Movementent();
    }

    private void Movementent()
    {

        Vector3 playerDir = transform.forward * movementInput.y * movementConfig.fowardSpeed + transform.right * movementInput.x * movementConfig.strafeSpeed;

        moveAmount = Vector3.SmoothDamp(moveAmount, playerDir, ref smoothMoveSpeed, smoothTime);

        rb.MovePosition(moveAmount + rb.position);
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
