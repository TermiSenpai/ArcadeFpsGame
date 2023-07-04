using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BillboardHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] Image billboardHealth;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }


    public void updateBillboardBar(float health)
    {
        pv.RPC("RPC_UpdateBar", RpcTarget.All, health);
    }

    [PunRPC]
    private void RPC_UpdateBar(float health)
    {
        billboardHealth.fillAmount = health;
    }
}
