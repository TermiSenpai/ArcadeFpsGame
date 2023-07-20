using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameOver : MonoBehaviour
{
    [SerializeField] PhotonTransformView ptv;
    [SerializeField] PlayerInput inputs;
    [SerializeField] Camera weaponCam;
    [SerializeField] CinemachineVirtualCamera mainCam;

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
        if (weaponCam != null)
            weaponCam.enabled = false;
        StartCoroutine(GameOverCamMovement());
    }

    IEnumerator GameOverCamMovement()
    {
        float timer = 9;
        while (timer > 0)
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + Time.deltaTime, mainCam.transform.position.z);
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
