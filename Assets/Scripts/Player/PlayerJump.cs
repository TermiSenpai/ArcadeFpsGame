using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerGravityConfig gravity;
    [SerializeField] Rigidbody rb;
    [SerializeField] private LayerMask ground;

    [SerializeField] float jumpForce = 8.5f;
    Vector3 speed;
   

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
                    jumpAction();
                }
                break;
        }
    }

    private void jumpAction()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public bool isGrounded()
    {
        Ray[] rays = new Ray[5]
        {
            new Ray(transform.position + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (transform.forward * 0.15f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.15f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.25f) + (Vector3.up * 0.02f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.25f) + (Vector3.up * 0.02f), Vector3.down),
        };

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
        Gizmos.DrawRay(transform.position + (transform.forward * 0.15f) + (Vector3.up * 0.02f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.15f) + (Vector3.up * 0.02f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.25f) + (Vector3.up * 0.02f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.25f) + (Vector3.up * 0.02f), Vector3.down);
    }
}
