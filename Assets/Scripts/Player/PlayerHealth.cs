using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region variables
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

    #endregion

    #region unity
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!pv.IsMine)
        {
            Destroy(health.gameObject);
        }

        SetMaxHealth();
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    #endregion

    #region custom methods
    void SetMaxHealth() => currentHealth = maxHealth;


    public void TakeDamage(float damage)
    {
        source.PlayOneShot(hitClip);
        pv.RPC(nameof(RPC_TackeDamage), pv.Owner, damage);
    }
    void UpdateHealthBar()
    {
        billboardHealth.UpdateBillboardBar(currentHealth / maxHealth);
        health.UpdateHealthBar(currentHealth / maxHealth);
        health.UpdateHealthTxt(currentHealth.ToString("00"));
    }


    private void PlayerDie()
    {
        playerManager.Die();
    }

    #endregion

    #region enumerators
    private IEnumerator RecoverHealth()
    {
        yield return new WaitForSeconds(secondsToStartRecovering);
        while (currentHealth < maxHealth)
        {
            currentHealth += Time.deltaTime;
            pv.RPC(nameof(RPC_RecoverHealth), pv.Owner);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    #endregion

    #region RPC

    [PunRPC]
    void RPC_TackeDamage(float damage, PhotonMessageInfo info)
    {
        if (recoverHealthCoroutine != null)
            StopCoroutine(recoverHealthCoroutine);

        recoverHealthCoroutine = StartCoroutine(RecoverHealth());

        currentHealth -= damage;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            PlayerDie();
            PlayerManager.Find(info.Sender).GetKill();
            KillManager.Instance.EnableKillInfo(info.Sender.NickName, playerManager.GetNickname());
        }
    }

    [PunRPC]
    void RPC_RecoverHealth()
    {
        currentHealth += Time.deltaTime;

        if (currentHealth >= maxHealth)
            SetMaxHealth();

        UpdateHealthBar();
    }

    #endregion
}
