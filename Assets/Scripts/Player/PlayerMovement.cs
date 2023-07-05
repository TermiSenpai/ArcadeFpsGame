using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig config;
    CharacterController controller;
    PlayerJump player;

    [Header("Inputs and movement")]
    private Vector2 movementInput;
    Vector3 moveDirection;
    Vector3 currentSpeed;

    PhotonView pv;
    Rigidbody rb;

    private void Awake()
    {
        player = GetComponent<PlayerJump>();
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        controller = GetComponent<CharacterController>();
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

        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        moveDirection = moveDirection.normalized;

        // Si hay movimiento
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calcular la velocidad objetivo
            Vector3 velocidadObjetivo = moveDirection * config.maxSpeed;

            // Calcular la aceleración
            currentSpeed = Vector3.Lerp(currentSpeed, velocidadObjetivo, config.acceleration * Time.deltaTime);
        }
        else
        {
            // Si no hay movimiento, detener al personaje
            currentSpeed = Vector3.zero;
        }        

        controller.Move(currentSpeed * config.movSpeed * Time.deltaTime);

        //if (player.isGrounded())
        //    rb.AddForce(moveDirection.normalized * config.movSpeed * config.movMultiplier, ForceMode.Acceleration);
        //else if(!player.isGrounded())
        //    rb.AddForce(moveDirection.normalized * config.movSpeed * config.airMovMultiplier, ForceMode.Acceleration);


    }

    private float applyGravity()
    {
        return Physics.gravity.y * Time.deltaTime;
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
