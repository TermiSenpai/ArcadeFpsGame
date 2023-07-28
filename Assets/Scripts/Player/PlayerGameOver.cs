using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerGameOver : MonoBehaviour
{
    #region Variables
    PhotonView pv;
    [Header("References")]
    [SerializeField] PhotonTransformView transformView;
    [SerializeField] Camera weaponCam;
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] MonoBehaviour[] scripts;
    
    [Header("Speed")]
    [SerializeField] float verticalSpeed = 1.0f;

    #endregion

    #region Unity
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

    #endregion

    #region Custom
    void DisableScripts()
    {
        foreach(var script in scripts)
        {
            script.enabled = false;
        }
    }

    void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DisableScripts(); 
        if (weaponCam != null)
            weaponCam.enabled = false;

        StartCoroutine(GameOverCamMovement());
    }

    #endregion

    #region Enumerator
    IEnumerator GameOverCamMovement()
    {
        float timer = 9;
       

        while (timer > 0)
        {
            mainCam.transform.position += Time.deltaTime * verticalSpeed * Vector3.up;
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    #endregion
}
