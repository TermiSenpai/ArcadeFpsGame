using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    PhotonView pv;
    PlayerManager playerManager;
    private const float maxHealth = 100f;
    private float currentHealth;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int) pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {
        pv.RPC("RPC_TackeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TackeDamage(float damage)
    {
        if (!pv.IsMine) return;



        currentHealth -= damage;

        if(currentHealth <= 0) playerDie();
    }

    private void playerDie()
    {
        playerManager.die();
    }
}
