using UnityEngine;
using UnityEngine.InputSystem;


public enum State
{
    gaming,
    paused
}
public class PlayerIngameSettings : MonoBehaviour
{
    
    #region Variables
    [SerializeField] private State currentState;
    [SerializeField] MonoBehaviour[] controlsScripts;
    #endregion

    #region custom
    public State GetState()
    {
        return currentState;
    }

    private void ContinueGame()
    {
        currentState = State.gaming;
        InGameMenu.Instance.TogglePauseMenu(false);
    }

    void PauseMenu()
    {
        currentState = State.paused;
        InGameMenu.Instance.TogglePauseMenu(true);
    }

    #endregion

    #region Input
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
    #endregion
}
