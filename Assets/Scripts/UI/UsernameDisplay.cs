using Photon.Pun;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPv;
    [SerializeField] TMP_Text txt;

    private void Start()
    {
        txt.text = playerPv.Owner.NickName;
    }
}
