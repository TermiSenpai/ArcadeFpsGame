using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameOver : MonoBehaviour
{
    PhotonView pv;
    [SerializeField] PhotonTransformView ptv;
    [SerializeField] PlayerInput inputs;
    [SerializeField] Camera weaponCam;
    [SerializeField] CinemachineVirtualCamera mainCam;

    private void Awake()
    {
        pv =GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!pv.IsMine)
        {
            Destroy(this);
        }
    }
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            mainCam.transform.position += Vector3.up * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
