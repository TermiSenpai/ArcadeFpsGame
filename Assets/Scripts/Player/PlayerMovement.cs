using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] PlayerMovConfig movementConfig;
    [SerializeField] CharacterController controller;
    Vector2 movementInput;

    private void LateUpdate()
    {
        Movementent();
    }

    private void Movementent()
    {
        Vector3 playerDir = transform.forward * movementInput.y + transform.right * movementInput.x;
        playerDir *= movementConfig.MovSpeed;
        playerDir.y = controller.velocity.y;

        controller.Move(playerDir * Time.deltaTime);
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
