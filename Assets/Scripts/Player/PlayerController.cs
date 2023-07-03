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
    [SerializeField] PlayerCam playerCam;
    [SerializeField] PlayerMovement playerMov;
    [SerializeField] PlayerJump playerJump;
    public MovementState state;
    public bool IsGrounded()
    {
        return playerJump.IsGrounded();
    }

    public void StateHandler()
    {
        //State - walking
        if (playerJump.IsGrounded())
        {
            state = MovementState.walking;
        }

        // State - air
        else if (!playerJump.IsGrounded())
        {
            state = MovementState.air;
        }
    }

    private void LateUpdate()
    {
        StateHandler();
    }
}
