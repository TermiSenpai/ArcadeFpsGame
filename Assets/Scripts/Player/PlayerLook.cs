using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform CamHolder;
    [SerializeField] private PlayerSensConfig sensConfig;
    private float camRot;

    //Posición del ratón
    private Vector2 mouseDelta;

    private int maxY = 90;
    private int minY = -85;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        cameraLook();
    }

    private void cameraLook()
    {
        camRot += mouseDelta.y * sensConfig.CamSensY;
        camRot = Mathf.Clamp(camRot, minY, maxY);
        CamHolder.localEulerAngles = new Vector3(-camRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * sensConfig.CamSensX, 0);

    }

    public void OnlookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
