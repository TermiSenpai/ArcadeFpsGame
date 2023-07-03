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

    //groundCheck
    [SerializeField] Transform groundCheck;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
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
}
