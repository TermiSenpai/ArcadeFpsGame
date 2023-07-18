using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SniperGun : Gun
{
    AudioSource source;

    [SerializeField] GameObject scopeOverlay;
    [SerializeField] GameObject crosshairOverlay;

    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] GameObject weaponCam;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] float reloadTimeDelay;
    int currentAmmo;
    PhotonView pv;
    Animator anim;

    Coroutine curCoroutine;
    GameObject impact;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public override void Use()
    {
        if (!canUse) return;

        if (currentAmmo == 0)
        {
            canUse = false;
            return;
        }
        shoot();
    }

    public override void Aim()
    {
        source.PlayOneShot(gunInfo.aimClip);
        anim.SetBool("Scoped", true);
        curCoroutine = StartCoroutine(nameof(enableScope));
    }

    public override void StopAim()
    {
        if (curCoroutine != null)
            StopCoroutine(curCoroutine);

        anim.SetBool("Scoped", false);
        disableScopeOverlay();
    }

    public void enableScopeOverlay()
    {
        cam.m_Lens.FieldOfView = 15f;
        scopeOverlay.SetActive(true);
        crosshairOverlay.SetActive(false);
        weaponCam.SetActive(false);
    }
    public void disableScopeOverlay()
    {
        cam.m_Lens.FieldOfView = 60f;
        scopeOverlay.SetActive(false);
        crosshairOverlay.SetActive(true);
        weaponCam.SetActive(true);
    }

    private IEnumerator enableScope()
    {
        yield return new WaitForSeconds(0.15f);
        enableScopeOverlay();
    }

    private void shoot()
    {

        source.PlayOneShot(gunInfo.useClip);

        anim.SetTrigger("Shoot");

        Ray r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, gunInfo.maxDistance, otherPlayerLayer))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
            weaponCoroutine = StartCoroutine(weaponCooldown());
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
        currentAmmo--;
    }


    public override void Reload()
    {
        if (curCoroutine != null)
            StopCoroutine(curCoroutine);

        source.PlayOneShot(gunInfo.reloadClip);
        curCoroutine = StartCoroutine(nameof(Recharge));
        anim.SetTrigger("Reload");
    }

    IEnumerator Recharge()
    {
        canUse = false;
        yield return new WaitForSeconds(reloadTimeDelay);
        currentAmmo = maxAmmo;
        canUse = true;
    }
    void impactPool()
    {
        pv.RPC("RPC_ImpactPool", RpcTarget.All);
    }

    // Reusable impactPos
    private Vector3 impactPos(Vector3 hitPos, Vector3 hitNormal)
    {
        return hitPos + hitNormal * 0.001f;
    }
    // Reusable impactRotation
    private Quaternion impactRotation(Vector3 hitNormal)
    {
        return Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation;
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
            Invoke(nameof(impactPool), 1.5f);

            //Destroy(impact, 1.5f);
            impact.transform.SetParent(colliders[0].transform);
            impact.transform.position = impactPos(hitPos, hitNormal);
            impact.transform.rotation = impactRotation(hitNormal);
        }
    }

    [PunRPC]
    void RPC_ImpactPool()
    {
        impact.transform.SetParent(transform);
        impact.transform.position = Vector3.zero;
        impact.SetActive(false);
    }
}
