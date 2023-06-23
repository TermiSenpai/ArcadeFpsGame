using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomName : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI roomName;
    private void OnEnable()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
    }
}
