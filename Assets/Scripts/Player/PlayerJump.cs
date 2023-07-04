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
    [SerializeField] CharacterController controller;

    [SerializeField] Vector3 velocity;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        applyGravity();
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, config.checkRadius, config.groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(groundCheck.position, config.checkRadius);
    }

    private void Jump()
    {
        if (!IsGrounded()) return;


    }

    void applyGravity()
    {

         velocity.y += config.gravity * Time.deltaTime;

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
}
