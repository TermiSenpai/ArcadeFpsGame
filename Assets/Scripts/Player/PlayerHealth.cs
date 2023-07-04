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
    [SerializeField] UIHealth health;
    [SerializeField] BillboardHealth billboardHealth;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int) pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if(!pv.IsMine)
            Destroy(health.gameObject);
        currentHealth = maxHealth;

        takeDamage(0f);
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

        billboardHealth.updateBillboardBar(currentHealth / maxHealth);
        health.updateHealthBar(currentHealth / maxHealth);
        health.updateHealthTxt(currentHealth.ToString("00"));

        if(currentHealth <= 0) playerDie();
    }

    private void playerDie()
    {
        playerManager.die();
    }
}
