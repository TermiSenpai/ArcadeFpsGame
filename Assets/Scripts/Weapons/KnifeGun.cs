using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGun : Gun
{

    AudioSource source;


    [SerializeField] CinemachineVirtualCamera cam;
    PhotonView pv;
    Animator anim;
    GameObject impact;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
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
        if (!canUse) return;

        source.PlayOneShot(gunInfo.useClip);
        Attack();

        weaponCoroutine = StartCoroutine(weaponCooldown());
    }

    public override void Aim()
    {
    }
    public override void StopAim()
    {
    }
    public override void Reload()
    {
    }

    private void OnDisable()
    {
        StopCoroutine(weaponCooldown());
    }

    private void OnEnable()
    {
        canUse = true;
    }

    private void Attack() => anim.SetTrigger("Attack");

    public void checkHit()
    {
        Vector3 center = cam.transform.position + cam.transform.forward * gunInfo.maxDistance;
        Collider[] colliders = Physics.OverlapSphere(center, 1f, otherPlayerLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider hitCollider in colliders)
            {
                // Creamos un rayo desde el centro de la cámara hacia el collider

                Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                r.origin = cam.transform.position;

                // Realizamos el Raycast para obtener información del impacto
                if (hitCollider.Raycast(r, out RaycastHit hitInfo, gunInfo.maxDistance))
                {
                    // Obtenemos el punto de impacto y la normal de superficie
                    Vector3 hitPoint = hitInfo.point;
                    Vector3 hitNormal = hitInfo.normal;
                    hitCollider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
                    pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hitPoint, hitNormal);
                }
            }
        }

    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPos, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPos, 0.3f);
        if (colliders.Length != 0)
        {
            if (impact == null)
                impact = Instantiate(bulletImpactPrefab, impactPos(hitPos, hitNormal), impactRotation(hitNormal));

            impact.SetActive(true);

            impact.transform.position = impactPos(hitPos, hitNormal);
            impact.transform.rotation = impactRotation(hitNormal);

        }
    }
}
