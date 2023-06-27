using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerJumpConfig jumpConfig;
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerGroundCheck groundCheck;
    Vector3 speed;
    bool groundState = true;
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (pv.IsMine)
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    jumpAction();
                    setGroundState(false);

                    break;
            }
    }

    private void jumpAction()
    {
        if (groundState)
            rb.AddForce(transform.up * jumpConfig.jumpForce, ForceMode.Impulse);

    }

    private void FixedUpdate()
    {
        applyGravity();
    }

    private void applyGravity()
    {
        Vector3 gravity = jumpConfig.gravityMultiplier * Physics.gravity;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    public bool isGrounded()
    {
        return groundState;
    }

    public void setGroundState(bool state) => groundState = state;

}
