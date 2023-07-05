using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{

    Rigidbody rb;
    PhotonView pv;
    [SerializeField] PlayerJumpConfig config;
    CharacterController controller;

    Vector3 velocity;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        applyGravity();

    }

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, config.checkRadius, config.groundLayer);
    }

    private void Jump()
    {
        if (!isGrounded()) return;

        velocity.y = Mathf.Sqrt(config.jumpForce * -2f * Physics.gravity.y);

        //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //rb.AddForce(transform.up * config.jumpForce, ForceMode.Impulse);
    }

    public void applyGravity()
    {
        if (isGrounded() && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += Physics.gravity.y * Time.deltaTime * config.gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);
        //rb.AddForce(gravity, ForceMode.Acceleration);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(groundCheck.position, config.checkRadius);
    }
}
