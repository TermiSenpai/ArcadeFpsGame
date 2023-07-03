using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public PlayerSensConfig config;
    [SerializeField] Transform cameraHolder;
    public Transform orientation;

    float xRotation;
    float yRotation;

    private Vector2 camInput;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!pv.IsMine)
        {
            Camera[] cameras = cameraHolder.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                Destroy(c.gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        if (pv.IsMine)
            playerLook();
    }

    void playerLook()
    {
        float mouseX = camInput.x * config.CamSensX;
        float mouseY = camInput.y * config.CamSensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, config.minY, config.maxY);

        cameraHolder.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        transform.eulerAngles += new Vector3(0, camInput.x * config.CamSensX, 0);

    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
    }

}
