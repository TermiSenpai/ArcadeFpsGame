using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu Instance;

    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void togglePauseMenu(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
        pauseMenu.SetActive(value);
    }

    public bool isPauseMenuEnable()
    {
        return pauseMenu.activeInHierarchy;
    }

    public void leaveRoom()
    {
        Destroy(RoomManager.Instance.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return 0;

        SceneManager.LoadScene(0);

    }

    private void OnEnable()
    {
        GameTimer.timerFinishReleased += GameOver;
    }

    private void OnDisable()
    {
        GameTimer.timerFinishReleased -= GameOver;
    }

    private void GameOver()
    {
        Invoke(nameof(leaveRoom), 10f);
    }
}
