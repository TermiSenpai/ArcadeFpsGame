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
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void cameraLook()
    {

    }

    void LateUpdate()
    {
        
    }
}
