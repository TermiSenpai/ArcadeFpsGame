using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text txt;
    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        txt.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.joinRoom(info);
    }
}
