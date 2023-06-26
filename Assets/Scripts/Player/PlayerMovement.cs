using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] PlayerMovConfig movementConfig;
   [SerializeField] Rigidbody rb;

    Vector2 movementInput;

    private void Start()
    {
        rb.freezeRotation = true;
    }
    private void FixedUpdate()
    {
        Movementent();
    }

    private void Movementent()
    {
        Vector3 playerDir = transform.forward * movementInput.y + transform.right * movementInput.x;

        rb.AddForce(playerDir.normalized * movementConfig.MovSpeed * 10f, ForceMode.Force);
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
