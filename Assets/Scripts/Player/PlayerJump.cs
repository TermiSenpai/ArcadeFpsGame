using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] PlayerJumpConfig jumpConfig;
    Rigidbody rb;   
    bool isGrounded;
    float playerHeight = 2f;
    PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        playerHeight = transform.localScale.y;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (pv.IsMine)
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    jumpAction();

                    break;
            }
    }

    private void jumpAction()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);
        if (isGrounded)
            rb.AddForce(transform.up * jumpConfig.jumpForce, ForceMode.Impulse);

    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
            applyGravity();
    }

    private void applyGravity()
    {
        Vector3 gravity = jumpConfig.gravityMultiplier * Physics.gravity;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }


}
