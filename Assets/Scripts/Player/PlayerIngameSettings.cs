using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIngameSettings : MonoBehaviour
{

    InGameMenu settingsMenu;

    [SerializeField] PlayerCam playerCam;
    [SerializeField] PlayerJump playerJump;
    [SerializeField] PlayerMovement playerMov;

    private void Awake()
    {
        settingsMenu = FindObjectOfType<InGameMenu>();
    }

    private void toggleMenu()
    {
        bool toggle = !settingsMenu.pauseMenu.activeInHierarchy;
        togglePlayerControlls(toggle);        
        settingsMenu.togglePauseMenu(toggle);
    }

    private void togglePlayerControlls(bool value)
    {
        playerCam.enabled = !value;
        playerJump.enabled = !value;
        playerMov.enabled = !value;
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                toggleMenu();
                break;
        }
    }
}
