using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.openMenu("title");
        Debug.Log("Joined lobby");
    }
    public void CreateRoom(TMP_InputField roomName)
    {
        if (string.IsNullOrEmpty(roomName.text))
            return;

        PhotonNetwork.CreateRoom(roomName.text);

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = PlayerLimit.Instance.limit;

        MenuManager.Instance.openMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.openMenu("room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

}
