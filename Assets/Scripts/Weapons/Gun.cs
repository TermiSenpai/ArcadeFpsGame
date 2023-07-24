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

    public IEnumerator weaponCooldown()
    {
        canUse = false;
        yield return new WaitForSeconds(cooldownSeconds);
        canUse = true;
    }

    // Reusable impactPos
    protected Vector3 impactPos(Vector3 hitPos, Vector3 hitNormal)
    {
        return hitPos + hitNormal * 0.001f;
    }
    // Reusable impactRotation
    protected Quaternion impactRotation(Vector3 hitNormal)
    {
        return Quaternion.LookRotation(hitNormal, Vector3.up) * Quaternion.Euler(0f, 0f, 0f);
        //return Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation;
    }
}
