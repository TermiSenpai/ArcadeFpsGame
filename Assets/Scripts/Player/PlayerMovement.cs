using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;



    [SerializeField] PlayerMovConfig config;
    CharacterController controller;
    PlayerJump player;

    float curMovementSpeed;

    Vector2 movementInput;
    Vector3 moveDirection;
    Vector3 currentSpeed;

    PhotonView pv;

    private void Awake()
    {
        Instance = this;
        player = GetComponent<PlayerJump>();
        pv = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        controller.detectCollisions = true;
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        MovePlayer();
        controlSpeed();
    }

    private void controlSpeed()
    {
        if (player.isGrounded())
        {
            curMovementSpeed = config.groundSpeed;
        }

        else if (!player.isGrounded())
        {
            curMovementSpeed = config.airSpeed;
        }
    }

    private void MovePlayer()
    {
        // Normalized in PlayerInputs Processor
        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;

        // Si hay movimiento
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calcular la velocidad objetivo
            Vector3 objetiveSpeed = moveDirection * config.maxSpeed;

            // Calcular la aceleración
            currentSpeed = Vector3.Lerp(currentSpeed, objetiveSpeed, config.acceleration * Time.deltaTime);
        }
        else
        {
            switch (player.isGrounded())

            {
                case true:
                    // Si no hay movimiento, detener al personaje
                    currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.groundDeceleration * Time.deltaTime);
                    break;
                case false:
                    currentSpeed = Vector3.Lerp(currentSpeed, Vector3.zero, config.airDeceleration * Time.deltaTime);
                    break;
            }

        }
        controller.Move(currentSpeed * curMovementSpeed * Time.deltaTime);
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

