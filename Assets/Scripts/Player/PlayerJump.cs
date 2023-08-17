using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] PlayerIngameSettings settings;
    [SerializeField] PlayerJumpConfig config;
    PhotonView pv;
    CharacterController controller;

    Vector3 velocity;

    [Header("Ground check")]
    [SerializeField] Transform groundCheck;

    #endregion

    #region Unity
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (!pv.IsMine) Destroy(this);
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        ApplyGravity();

    }

    #endregion

    #region custom
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        // Calcula la gravedad ajustada según el multiplicador definido en la configuración
        float gravity = Physics.gravity.y * config.gravityMultiplier;

        // Calcula la diferencia en la posición vertical
        float displacementY = endPoint.y - startPoint.y;

        // Calcula la diferencia en las coordenadas horizontales (ignorando la altura)
        Vector3 displacementXZ = new(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        // Calcula la velocidad vertical inicial usando la ecuación de movimiento vertical bajo gravedad
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);

        // Calcula la velocidad horizontal usando la ecuación de movimiento horizontal
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        // Retorna la suma de las velocidades vertical y horizontal para obtener la velocidad total
        return velocityY + velocityXZ;
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, config.checkRadius, config.groundLayer);
    }

    private void Jump()
    {
        if (!IsGrounded()) return;

        velocity.y = Mathf.Sqrt(config.jumpForce * -2f * Physics.gravity.y);
    }

    public void ApplyGravity()
    {
        if (IsGrounded() && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += Physics.gravity.y * Time.deltaTime * config.gravityMultiplier;
        controller.Move(velocity * Time.deltaTime);
    }

    #endregion

    #region Input
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;

        if (settings.GetState() == State.paused) return;


        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump();
                break;
        }
    }

    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(groundCheck.position, config.checkRadius);
    }
}
