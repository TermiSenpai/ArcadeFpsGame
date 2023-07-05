using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    

    public void togglePauseMenu(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
        pauseMenu.SetActive(value);
    }

    public void leaveRoom()
    {
        PhotonNetwork.LoadLevel(0);
        //SceneManager.LoadScene(0);
        PhotonNetwork.LeaveRoom();
    }

}
