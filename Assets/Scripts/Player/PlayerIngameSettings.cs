using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIngameSettings : MonoBehaviour
{

    //InGameMenu settingsMenu;

    [SerializeField] MonoBehaviour[] controlsScripts;
  

    private void toggleMenu()
    {
        bool toggle = !InGameMenu.Instance.pauseMenu.activeInHierarchy;
        togglePlayerControlls(toggle);        
        InGameMenu.Instance.togglePauseMenu(toggle);
    }

    private void togglePlayerControlls(bool value)
    {
       foreach (var control in controlsScripts)
        {
            control.enabled = !value;
        }
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
