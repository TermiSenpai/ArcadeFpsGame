using Photon.Pun;
using UnityEngine;

public class PlayerBodyPart : MonoBehaviour, IDamageable
{
    [SerializeField] float partDamageMultiplier;

    PlayerHealth playerHP;
    PhotonView pv;
    private void Awake()
    {
        pv = GetComponentInParent<PhotonView>();
        playerHP = GetComponentInParent<PlayerHealth>();
    }

    void IDamageable.TakeDamage(float damage)
    {
        damage *= partDamageMultiplier;
        playerHP.TakeDamage(damage);
    }
}
