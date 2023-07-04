using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScoreboard : MonoBehaviour
{
    CanvasGroup group;
    private void Start()
    {
        group = FindObjectOfType<CanvasGroup>();
    }

    public void OnScoreboardInput(InputAction.CallbackContext context)
    {
        switch(context.phase) {
            case InputActionPhase.Performed:
                group.alpha = 1;
                break;

            case InputActionPhase.Canceled: 
                group.alpha = 0;    
                break;
        }

    }
}
