using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    gaming,
    paused
}
public class PlayerIngameSettings : MonoBehaviour
{

    //InGameMenu settingsMenu;
    [SerializeField] private State pausedState;
    [SerializeField] MonoBehaviour[] controlsScripts;

    public State GetState()
    {
        return pausedState;
    }

    private void ContinueGame()
    {
        pausedState = State.gaming;
        InGameMenu.Instance.TogglePauseMenu(false);
    }

    void PauseMenu()
    {
        pausedState = State.paused;
        InGameMenu.Instance.TogglePauseMenu(true);
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:

                if (InGameMenu.Instance.IsPauseMenuEnable())
                    ContinueGame();
                else PauseMenu();

                break;
        }
    }
}
