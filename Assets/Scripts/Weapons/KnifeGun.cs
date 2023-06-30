using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGun : Gun
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
        //    Destroy(cam.gameObject);
    }

    public override void Use()
    {
        shoot();
    }

    private void shoot()
    {
        Ray r = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, ((GunInfo)itemInfo).maxDistance))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(((GunInfo)itemInfo).damage);
        }
    }
}
