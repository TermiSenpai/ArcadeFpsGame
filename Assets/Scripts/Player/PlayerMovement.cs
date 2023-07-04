using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig config;
    PlayerJump player;

    [Header("Inputs and movement")]
    private Vector2 movementInput;
    Vector3 moveDirection;

    PhotonView pv;
    Rigidbody rb;

    private void Awake()
    {
        player = GetComponent<PlayerJump>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if (!pv.IsMine)
            Destroy(rb);

    }

    private void Update()
    {
        if (!pv.IsMine) return;

        MovePlayer();
        controlDrag();
    }

    private void controlDrag()
    {
        if (player.isGrounded())
        {
            config.movSpeed = 3.5f;
            rb.drag = config.groundDrag;
        }
        else if (!player.isGrounded())
        {
            config.movSpeed = 2;
            rb.drag = config.airDrag;
        }
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * movementInput.y  + transform.right * movementInput.x ;

        rb.AddForce(moveDirection.normalized * config.movSpeed * config.movMultiplier, ForceMode.Acceleration);

    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                movementInput = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Performed:
                movementInput = context.ReadValue<Vector2>();
                break;

            case InputActionPhase.Canceled:
                movementInput = Vector2.zero;
                break;
        }
    }
}
