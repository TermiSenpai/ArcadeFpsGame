using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrappling : MonoBehaviour
{
    #region variables
    [Header("References")]
    [SerializeField] PlayerIngameSettings settings;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform gunTip;
    [SerializeField] LayerMask whatIsGrappleable;
    [SerializeField] LineRenderer lr;
    PlayerMovement pm;
    PlayerJump pj;
    private PhotonView pv;


    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float overshootYAxis;
    private Vector3 grapplePoint;


    [Header("Cooldown")]
    [SerializeField] float grapplingCd;
    float grapplingCdTimer;
    bool isGrappling;


    #endregion

    #region unity
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        pm = GetComponent<PlayerMovement>();
        pj = GetComponent<PlayerJump>();
    }


    private void Update()
    {
        if (!pv.IsMine) return;


        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (!pv.IsMine) return;

        if (isGrappling)
            lr.SetPosition(0, gunTip.position);
    }
    #endregion

    #region Grapple
    void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;


        pm.isFreeze = true;
        isGrappling = true;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out RaycastHit hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelay);
        }
        else
        {
            grapplePoint = camTransform.position + camTransform.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelay);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    void ExecuteGrapple()
    {
        if (!pv.IsMine) return;

        pm.isFreeze = false;

        Vector3 lowestPoint = new(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), grappleDelay);
    }

    void StopGrapple()
    {
        if (!pv.IsMine) return;
        isGrappling = false;
        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
        pm.isFreeze = false;
        pm.isActiveGrapple = false;

    }
    #endregion

    #region Input

    public void OnGrappleInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        if (settings.GetState() == State.paused) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                StartGrapple();
                break;
        }
    }

    #endregion
}
