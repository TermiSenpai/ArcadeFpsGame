using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrappling : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform gunTip;
    [SerializeField] LayerMask whatIsGrappleable;
    [SerializeField] LineRenderer lr;
    PlayerMovement pm;
    private PhotonView pv;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    [SerializeField] float grapplingCd;
    float grapplingCdTimer;

    // Temporal to see in editor
    [SerializeField] bool isGrappling;
    #region unity
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        pm = GetComponent<PlayerMovement>();
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
    void startGrapple()
    {
        Debug.Log("Start");
        if (grapplingCdTimer > 0) return;

        pm.isFreeze = true;
        isGrappling = true;
        Debug.Log("start raycast");
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(executeGrapple), grappleDelay);
            Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            grapplePoint = camTransform.position + camTransform.forward * maxGrappleDistance;

            Invoke(nameof(stopGrapple), grappleDelay);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    void executeGrapple()
    {
        Debug.Log("execute");
        if (!pv.IsMine) return;

        pm.isFreeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        Debug.Log("Jump");
        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(stopGrapple), grappleDelay);
    }

    void stopGrapple()
    {
        if (!pv.IsMine) return;
        isGrappling = false;
        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
        pm.isFreeze = false;
        pm.isActiveGrapple = false;
        Debug.Log("Stop");
    }
    #endregion

    #region Input

    public void OnGrappleInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                startGrapple();
                break;
        }
    }

    #endregion
}
