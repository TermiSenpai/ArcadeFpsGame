using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    #region Variables
    public static InGameMenu Instance;
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject exitBtn;
    [SerializeField] private GameObject restartBtn;

    #endregion

    #region Unity
    private void Awake()
    {
        Instance = this;
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
    public void TogglePauseMenu(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
        pauseMenu.SetActive(value);
    }

    public bool IsPauseMenuEnable()
    {
        return pauseMenu.activeInHierarchy;
    }

    private void GameOver()
    {
        Invoke(nameof(EnableBtns), 5f);
    }

    void EnableBtns()
    {
        exitBtn.SetActive(true);
        restartBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    #endregion

    #region Network
    public void LeaveRoom()
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
    public void RestartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomManager.Instance.LoadLevel();
    }
    #endregion
}
