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
    [SerializeField] float secondsToStartRecovering = 5f;

    Coroutine recoverHealthCoroutine;

    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitClip;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!pv.IsMine)
        {
            //Destroy(billboardHealth);
            Destroy(health.gameObject);
        }

        setMaxHealth();
        updateHealthBar();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        updateHealthBar();
    }

    void setMaxHealth() => currentHealth = maxHealth;


    public void takeDamage(float damage)
    {
        source.PlayOneShot(hitClip);
        pv.RPC(nameof(RPC_TackeDamage), pv.Owner, damage);
    }

    private IEnumerator recoverHealth()
    {
        yield return new WaitForSeconds(secondsToStartRecovering);
        while (currentHealth < maxHealth)
        {
            currentHealth += Time.deltaTime;
            pv.RPC(nameof(RPC_RecoverHealth), pv.Owner);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    [PunRPC]
    void RPC_TackeDamage(float damage, PhotonMessageInfo info)
    {
        if (recoverHealthCoroutine != null)
            StopCoroutine(recoverHealthCoroutine);

        recoverHealthCoroutine = StartCoroutine(recoverHealth());

        currentHealth -= damage;

        updateHealthBar();

        if (currentHealth <= 0)
        {
            playerDie();
            PlayerManager.Find(info.Sender).getKill();
        }
    }

    [PunRPC]
    void RPC_RecoverHealth()
    {
        currentHealth += Time.deltaTime;

        if (currentHealth >= maxHealth)
            setMaxHealth();

        updateHealthBar();
    }

    void updateHealthBar()
    {
        billboardHealth.updateBillboardBar(currentHealth / maxHealth);
        health.updateHealthBar(currentHealth / maxHealth);
        health.updateHealthTxt(currentHealth.ToString("00"));
    }

    
    private void playerDie()
    {        
        playerManager.die();
    }
}
