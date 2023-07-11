using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Start()
    {
        if (!pv.IsMine) Destroy(this);
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        applyGravity();

    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        Debug.Log("calculate");

        Debug.Log("calculating");
        float gravity = Physics.gravity.y * config.gravityMultiplier;
        float displacementY = endPoint.y - startPoint.y;

        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);

        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityY + velocityXZ;
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
