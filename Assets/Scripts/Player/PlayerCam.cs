using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public PlayerSensConfig config;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private Vector2 camInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        float mouseX = camInput.x * config.CamSensX;
        float mouseY = camInput.y * config.CamSensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, config.minY, config.maxY);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
    }
    
}
