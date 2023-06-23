using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerGravityConfig gravity;
    [SerializeField] CharacterController controller;
    [SerializeField] private LayerMask ground;

    [SerializeField] float jumpHeight = 8.5f;
    Vector3 speed;
   

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        
    }


    public void OnJumpInput (InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (isGrounded())
                {
                    speed.y = jumpHeight;
                }
                break;
        }
    }


    private void FixedUpdate()
    {
        applyGrav();
    }

    private void applyGrav()
    {
        if (isGrounded() && speed.y < 0)
            speed.y = -2f;
        speed.y += gravity.gravity * Time.deltaTime;
        controller.Move(speed * Time.deltaTime);
    }

    public bool isGrounded()
    {
        Ray[] rays = new Ray[1]
        {new Ray(transform.position + (Vector3.up * 0.02f), Vector3.down)};

        foreach (Ray r in rays)
            if (Physics.Raycast(r, 1.2f, ground))
            {
                Debug.Log("Ready");
                return true;
            }
            return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.02f), Vector3.down);
    }
}
