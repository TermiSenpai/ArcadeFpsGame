using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        //if (!pv.IsMine)
        //   Destroy(handsGameobject);
    }

    public override void Use()
    {
        shoot();
    }

    private void shoot()
    {
        Ray r = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(((GunInfo)itemInfo).damage);
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
