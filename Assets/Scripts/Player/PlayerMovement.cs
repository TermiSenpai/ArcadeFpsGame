using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovConfig movementConfig;
    Rigidbody rb;

    Vector2 movementInput;

    PhotonView pv;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!pv.IsMine)
            Destroy(rb);
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        Movementent();
        controlDrag();
    }

    private void Movementent()
    {

        Vector3 playerDir = transform.forward * movementInput.y * movementConfig.fowardSpeed + transform.right * movementInput.x * movementConfig.strafeSpeed;

        rb.AddForce(playerDir.normalized * movementConfig.movementMultiplier, ForceMode.Acceleration);
    }

    private void controlDrag()
    {
        rb.drag = movementConfig.rbDrag;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (pv.IsMine)
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
