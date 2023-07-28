using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : Item
{
    public GameObject bulletImpactPrefab;
    public bool canUse = true;
    public float cooldownSeconds;
    public Coroutine weaponCoroutine;
    public LayerMask otherPlayerLayer;

    public abstract override void Use();

    public IEnumerator WeaponCooldown()
    {
        canUse = false;
        yield return new WaitForSeconds(cooldownSeconds);
        canUse = true;
    }

    protected Vector3 ImpactPos(Vector3 hitPos, Vector3 hitNormal)
    {
        return hitPos + hitNormal * 0.001f;
    }

    protected Quaternion ImpactRotation(Vector3 hitNormal)
    {
        return Quaternion.LookRotation(hitNormal, Vector3.up) * Quaternion.Euler(0f, 0f, 0f);
    }
}
