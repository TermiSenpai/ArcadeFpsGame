using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig config;
    PlayerController playerController;
    public Transform orientation;
    [SerializeField] CharacterController controller;

    [Header("Inputs and movement")]
    private Vector2 movementInput;
    PhotonView pv;
    Vector3 moveDirection;
    Rigidbody rb;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if(!pv.IsMine)        
            Destroy(rb);
        
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        //On ground
         if(playerController.IsGrounded())
            controller.Move(moveDirection.normalized * config.moveSpeed * config.movMultiplier * Time.deltaTime);

        // On air
        else if(!playerController.IsGrounded())
            controller.Move(moveDirection.normalized * config.moveSpeed * config.movMultiplier * config.airMovMultiplier * Time.deltaTime);
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                movementInput = context.ReadValue<Vector2>();
                break;

            case InputActionPhase.Canceled:
                movementInput = Vector2.zero;
                break;
        }
    }
}
