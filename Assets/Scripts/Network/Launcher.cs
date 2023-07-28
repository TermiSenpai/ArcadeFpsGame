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
    [SerializeField] GameObject startGameBtn;
    [SerializeField] TMP_Text Title;

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
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined lobby");

        PhotonNetwork.NickName = GetNickName();
    }

    #endregion

    #region Create
    // Create room based in input field text
    public void CreateRoom(TMP_InputField roomName)
    {
        if (string.IsNullOrEmpty(roomName.text))
            return;

        RoomOptions roomOptions = new()
        {
            MaxPlayers = PlayerLimit.Instance.limit
        };

        roomName.text = RoomUpperName(roomName.text);
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);

        MenuManager.Instance.OpenMenu("loading");
    }
    // if failed, show error
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowError(message);
    }

    #endregion

    #region Join room

    // Join room based in list
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    // On join, open room and update playerLists
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");

        OnPlayerListUpdate();
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    // instantiate player on enter room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    // If joining room fail, show error
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowError(message);
    }

    #endregion

    #region Leave room
    // Leave current room
    public void LeaveRoom()
    {
        MenuManager.Instance.OpenMenu("loading");
        PhotonNetwork.LeaveRoom();
    }

    // Open title menu on left room
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    #endregion

    #region Errors

    // Show error messages
    private void ShowError(string message)
    {
        MenuManager.Instance.OpenMenu("error");
        errorTxt.text = $"Error: {message}";
    }

    #endregion

    #region Update
    // Update room list
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform T in roomListContent)
            Destroy(T.gameObject);


        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
                continue;

            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
        }

    }

    // Update player list
    private void OnPlayerListUpdate()
    {
        foreach (Transform child in playerListContent)

            Destroy(child.gameObject);

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
        }

        Title.text = $"{PhotonNetwork.CurrentRoom.Name}";
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    #endregion

    #region Starting

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion

    #region Custom methods
    // transform any room name in UPPERCASE
    private string RoomUpperName(string name)
    {
        return name.ToUpper();
    }
    // Exit game
    public void ExitGame()
    {
        Application.Quit();
    }

    // Obtain nickname based in have play anytime or not
    private string GetNickName()
    {
        if (!PlayerPrefs.HasKey("Nickname"))
            return $"Player {Random.Range(1, 20)}";


        return PlayerPrefs.GetString("Nickname");
    }

    #endregion

}
