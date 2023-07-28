using Photon.Pun;
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
