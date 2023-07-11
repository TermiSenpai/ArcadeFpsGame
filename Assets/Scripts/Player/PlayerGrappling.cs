using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform gunTip;
    [SerializeField] LayerMask whatIsGrappleable;
    [SerializeField] LineRenderer lr;
    private PhotonView pv;

    [Header("Grappling")]
    [SerializeField] private float maxGrappleDistance;
    [SerializeField] private float grappleDelay;

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

    private void Start()
    {
        if(!pv.IsMine)
        {

        }
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        if(grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime; 
    }

    private void LateUpdate()
    {
        if(!pv.IsMine) return;

        if(isGrappling)
            lr.SetPosition(0, gunTip.position);
    }
    #endregion

    #region Grapple
    void startGrapple()
    {
        if (grapplingCdTimer > 0) return;
        isGrappling = true;

        RaycastHit hit;
        if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxGrappleDistance, whatIsGrappleable)) 
        {
            grapplePoint = hit.point;
            Invoke(nameof(executeGrapple), grappleDelay);
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

    }

    void stopGrapple()
    {
        isGrappling = false;
        grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }
    #endregion

    #region Input

    public void OnGrappleInput(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                startGrapple();
                break;
        }
    }

    #endregion
}
