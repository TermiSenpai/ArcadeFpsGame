using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minJumpHeight = 1.5f;
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;
    [SerializeField] float wallRunJumpMultiplier;
    [SerializeField] PlayerJump playerJumping;

    private Rigidbody rb;
    private PhotonView pv;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    bool wallLeft = false;
    bool wallRight = false;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!pv.IsMine)
            Destroy(rb);
    }
    private void Update()
    {
        if (!pv.IsMine) return;

        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                startWallRun();
                Debug.Log("left");
            }
            else if (wallRight)
            {
                startWallRun();
                Debug.Log("Right");
            }
            else stopWallRun();
        }
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }


    void startWallRun()
    {
        playerJumping.enabled = false;
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
    }

    void stopWallRun()
    {
        playerJumping.enabled = true;
        rb.useGravity = true;
    }

    void Jump()
    {
        if (wallLeft)
        {
            Vector3 wallRunJumpDir = transform.up + leftWallHit.normal * 2;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDir * wallRunJumpForce * wallRunJumpMultiplier, ForceMode.Force);
        }
        if (wallRight)
        {
            Vector3 wallRunJumpDir = transform.up + rightWallHit.normal * 2;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDir * wallRunJumpForce * wallRunJumpMultiplier, ForceMode.Force);
        }
    }

    public void onJumpInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump();
                break;
        }
    }
}
