using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerJumpConfig jumpConfig;
    [SerializeField] Transform groundCheck;
    Rigidbody rb;

    public float distance = 0.1f;

    PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (pv.IsMine)
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    jumpAction();

                    break;
            }
    }

    private void jumpAction()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpConfig.jumpForce, ForceMode.Impulse);
        }

    }

    private void FixedUpdate()

    {
        if (pv.IsMine)
            applyGravity();
    }

    private void applyGravity()
    {
        Vector3 gravity = jumpConfig.gravityMultiplier * Physics.gravity;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, jumpConfig.groundDistance, jumpConfig.ground); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, distance, 0), jumpConfig.groundDistance);
    }


}
