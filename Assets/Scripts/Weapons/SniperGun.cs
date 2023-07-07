using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGun : Gun
{
    [SerializeField] GameObject scopeOverlay; 
    [SerializeField] GameObject crosshairOverlay; 
    [SerializeField] Camera cam;
    [SerializeField] GameObject weaponCam;
    PhotonView pv;
    Animator anim;

    

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }   

    public override void Use()
    {
        if (!canUse) return;

        shoot();
    }

    public override void Aim()
    {
        anim.SetBool("Scoped", true);
    }

    public override void StopAim()
    {
        anim.SetBool("Scoped", false);        
    }

    public void enableScopeOverlay()
    {
        cam.fieldOfView = 15f;
        scopeOverlay.SetActive(true);
        crosshairOverlay.SetActive(false);
        weaponCam.SetActive(false);
    }
    public void disableScopeOverlay()
    {
        cam.fieldOfView = 60f;
        scopeOverlay.SetActive(false);
        crosshairOverlay.SetActive(true);
        weaponCam.SetActive(true);
    }

    private void shoot()
    {
        Ray r = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(((GunInfo)itemInfo).damage);
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
