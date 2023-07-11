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
}
