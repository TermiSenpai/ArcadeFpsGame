using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(pv.IsMine)
        {
            createController();
        }
    }

    void createController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhothonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
