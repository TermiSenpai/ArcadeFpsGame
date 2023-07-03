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
    PlayerController playerController;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        applyGravity();
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, config.checkRadius, config.groundLayer);
    }

    private void Jump()
    {
        if (!IsGrounded()) return;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * config.jumpForce, ForceMode.Impulse);
    }

    void applyGravity()
    {
        Vector3 gravity = config.gravityMultiplier * Physics.gravity;
        rb.AddForce(gravity, ForceMode.Acceleration);
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
