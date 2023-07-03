using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    walking,
    air
}

public class PlayerController : MonoBehaviour
{
    PhotonView pv;
    PlayerMovement movement;
    PlayerJump jumping;
    [SerializeField] MovementState state;


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        movement = GetComponent<PlayerMovement>();
        jumping = GetComponent<PlayerJump>();
    }

    private void LateUpdate()
    {
        if (!pv.IsMine) return;

        stateHandler();
    }

    public bool IsGrounded()
    {
        return jumping.IsGrounded();
    }

    private void stateHandler()
    {
        // State walking
        switch (jumping.IsGrounded())
        {
            case true:
                state = MovementState.walking;
                break;

            case false:
                state = MovementState.air;
                break;
        }
    }
}
