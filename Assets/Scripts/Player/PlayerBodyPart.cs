using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    public void takeDamage(float damage)
    {
        damage *= partDamageMultiplier;
        playerHP.TakeDamage(damage);
    }
}
