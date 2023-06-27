using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform CamHolder;
    [SerializeField] private PlayerSensConfig sensConfig;
    private float camRot;

    //Movimiento del ratón
    private Vector2 mouseDelta;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!pv.IsMine)
            Destroy(CamHolder.GetComponentInChildren<Camera>().gameObject);
    }

    void LateUpdate()
    {
        if (pv.IsMine)
            cameraLook();
    }

    private void cameraLook()
    {
        camRot += mouseDelta.y * sensConfig.CamSensY;
        camRot = Mathf.Clamp(camRot, sensConfig.minY, sensConfig.maxY);
        CamHolder.localEulerAngles = new Vector3(-camRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * sensConfig.CamSensX, 0);

    }

    public void OnlookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
