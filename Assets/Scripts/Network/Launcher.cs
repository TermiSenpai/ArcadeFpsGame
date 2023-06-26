using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    #region Variables

    [SerializeField] TextMeshProUGUI errorTxt;
    [Header("Find Room")]
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [Header("Inside Room")]
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListPrefab;

    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Connect to master
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.openMenu("title");
        Debug.Log("Joined lobby");

        PhotonNetwork.NickName = getNickName();
        Debug.Log(PhotonNetwork.NickName);
    }

    #endregion

    #region Create
    public void CreateRoom(TMP_InputField roomName)
    {
        if (string.IsNullOrEmpty(roomName.text))
            return;

        roomName.text = roomUpperName(roomName.text);
        PhotonNetwork.CreateRoom(roomName.text);

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = PlayerLimit.Instance.limit;

        MenuManager.Instance.openMenu("loading");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        showError(message);
    }

    #endregion

    #region Join room

    public void joinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.openMenu("loading");
    }

    public void joinRoom(TMP_InputField roomName)
    {
        if (string.IsNullOrEmpty(roomName.text))
            return;

        roomName.text = roomUpperName(roomName.text);

        PhotonNetwork.JoinRoom(roomName.text);
        MenuManager.Instance.openMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.openMenu("room");

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        showError(message);
    }

    #endregion

    #region Leave room

    public void LeaveRoom()
    {
        MenuManager.Instance.openMenu("loading");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.openMenu("title");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("leave room");
    }

    #endregion

    #region Errors

    private void showError(string message)
    {
        MenuManager.Instance.openMenu("error");
        errorTxt.text = $"Error: {message}";
    }

    #endregion

    #region Update
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform T in roomListContent)
            Destroy(T.gameObject);


        foreach (var room in roomList)
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);

    }

    #endregion

    #region Custom methods
    private string roomUpperName(string name)
    {
        return name.ToUpper();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private string getNickName()
    {
        if (!PlayerPrefs.HasKey("Nickname"))
            return $"Player {Random.Range(1, 20)}";


        return PlayerPrefs.GetString("Nickname");
    }

    #endregion

}
