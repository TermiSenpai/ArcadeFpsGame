using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView pv;
    Animator anim;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if (pv.IsMine)
            gameObject.layer = 8;
    }

    public override void Use()
    {
        Attack();
    }

    public override void Aim()
    {
    }
    public override void StopAim()
    {
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public void checkHit()
    {
        Ray r = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, ((GunInfo)itemInfo).maxDistance))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(((GunInfo)itemInfo).damage);
            weaponCoroutine = StartCoroutine(weaponCooldown());
        }

    }
}
