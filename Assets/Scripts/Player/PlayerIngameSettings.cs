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

    private void continueGame()
    {
        pausedState = State.gaming;
        InGameMenu.Instance.togglePauseMenu(false);
    }

    void pauseMenu()
    {
        pausedState = State.paused;
        InGameMenu.Instance.togglePauseMenu(true);
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:

                if (InGameMenu.Instance.isPauseMenuEnable())
                    continueGame();
                else pauseMenu();

                break;
        }
    }
}
