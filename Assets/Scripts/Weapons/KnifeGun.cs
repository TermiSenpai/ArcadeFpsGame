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

    private void Attack() => anim.SetTrigger("Attack");

    public void checkHit()
    {
        Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, gunInfo.maxDistance, otherPlayerLayer))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            Debug.Log(hit.collider.gameObject.name);
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
            if (impact == null)
                impact = Instantiate(bulletImpactPrefab, impactPos(hitPos, hitNormal), impactRotation(hitNormal));

            impact.SetActive(true);

            // Cancel invoke
            CancelInvoke();
            // Start new invoke with renewed time
            //Invoke(nameof(impactPool), 1.5f);

            //Destroy(impact, 1.5f);
            //impact.transform.SetParent(colliders[0].transform);
            impact.transform.position = impactPos(hitPos, hitNormal);
            impact.transform.rotation = impactRotation(hitNormal);

        }
    }

    [PunRPC]
    void RPC_ImpactPool()
    {
        if (impact == null) return;
        //impact.transform.SetParent(transform);
        impact.transform.position = Vector3.zero;
        impact.SetActive(false);
    }

    void impactPool()
    {
        pv.RPC("RPC_ImpactPool", RpcTarget.All);
    }
}
