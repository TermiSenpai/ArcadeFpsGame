using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameOver : MonoBehaviour
{
    [SerializeField] PhotonTransformView ptv;
    [SerializeField] PlayerInput inputs;

    private void OnEnable()
    {
        GameTimer.timerFinishReleased += GameOver;
    }

    private void OnDisable()
    {
        GameTimer.timerFinishReleased -= GameOver;
    }

    void GameOver()
    {
        ptv.enabled = false;
        inputs.enabled = false;
    }
}
