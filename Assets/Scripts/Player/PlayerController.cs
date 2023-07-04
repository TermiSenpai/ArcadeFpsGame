using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    PhotonView pv;
    PlayerMovement movement;
    PlayerJump jumping;


    private void Awake()
    {
        jumping = GetComponent<PlayerJump>();
    }

    public bool IsGrounded()
    {
        return jumping.isGrounded();
    }
}
