using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    PhotonView pv;
    [SerializeField] PlayerJumpConfig config;
    CharacterController controller;

    Vector3 velocity;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;



    private void Awake()
    {
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
    }

    public void applyGravity()
    {
        if (isGrounded() && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += Physics.gravity.y * Time.deltaTime * config.gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);
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
