using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
    }

    public void takeDamage(float damage)
    {
        pv.RPC("RPC_TackeDamage", RpcTarget.All, damage, PhotonNetwork.NickName);
    }

    [PunRPC]
    void RPC_TackeDamage(float damage, string nick)
    {
        if (!pv.IsMine) return;

        Debug.Log($"damaged by: {nick}");
    }
}
