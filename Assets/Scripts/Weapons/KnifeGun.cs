using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGun : Gun
{
    [SerializeField] CinemachineVirtualCamera cam;
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
        Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, gunInfo.maxDistance))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
            weaponCoroutine = StartCoroutine(weaponCooldown());
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }

    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPos, Vector3 hitNormal)
    {
        //TODO
        // Change instantiate for gameobject enable
        Collider[] colliders = Physics.OverlapSphere(hitPos, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject impact = Instantiate(bulletImpactPrefab, hitPos + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(impact, 5f);
            impact.transform.SetParent(colliders[0].transform);

        }



    }
}
