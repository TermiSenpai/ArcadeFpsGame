using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PhotonView pv;
    PlayerManager playerManager;
    private const float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] UIHealth health;
    [SerializeField] BillboardHealth billboardHealth;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip deathClip;

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
        source.PlayOneShot(hitClip);
        pv.RPC(nameof(RPC_TackeDamage), pv.Owner, damage);
    }

    [PunRPC]
    void RPC_TackeDamage(float damage, PhotonMessageInfo info)
    {

        currentHealth -= damage;

        billboardHealth.updateBillboardBar(currentHealth / maxHealth);
        health.updateHealthBar(currentHealth / maxHealth);
        health.updateHealthTxt(currentHealth.ToString("00"));

        if (currentHealth <= 0)
        {
            playerDie();
            PlayerManager.Find(info.Sender).getKill();
        }
    }

    private void playerDie()
    {
        source.PlayOneShot(deathClip);
        playerManager.die();
    }
}
