using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text RoomName;
    [SerializeField] TMP_Text RoomPlayers;
    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        RoomName.text = _info.Name;
        RoomPlayers.text = $"{_info.PlayerCount}/{_info.MaxPlayers}";
    }

    public void OnClick()
    {
        Launcher.Instance.joinRoom(info);
    }
}
