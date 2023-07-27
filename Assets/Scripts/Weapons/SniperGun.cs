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

    Coroutine aimCoroutine;
    Coroutine reloadCoroutine;
    GameObject impact;

    [SerializeField] UIAmmo ammoUI;
    [SerializeField] float timeToScope;
    bool isReloading = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        source.maxDistance = 25f;
        source.minDistance = 10f;
    }

    public override void Use()
    {
        if (currentAmmo == 0) noAmmo();
        if (!canUse) return;


        shoot();
    }

    public override void Aim()
    {
        source.PlayOneShot(gunInfo.aimClip);
        anim.SetBool("Scoped", true);
        aimCoroutine = StartCoroutine(nameof(enableScope));
    }

    public override void StopAim()
    {
        if (aimCoroutine != null)
            StopCoroutine(aimCoroutine);

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
        yield return new WaitForSeconds(timeToScope);
        enableScopeOverlay();
    }

    private void shoot()
    {
        anim.SetTrigger("Shoot");
        Ray r;

        // if scope, have 100% precision
        if (anim.GetBool("Scoped"))
            r = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        // else, have random ray
        else
            r = Camera.main.ViewportPointToRay(new Vector3(Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f)));


        r.origin = cam.transform.position;

        if (Physics.Raycast(r, out RaycastHit hit, gunInfo.maxDistance, otherPlayerLayer))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
            weaponCoroutine = StartCoroutine(weaponCooldown());
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
        currentAmmo--;
        ammoUI.updateAmmoTxt(currentAmmo, maxAmmo);
    }


    public override void Reload()
    {
        if (isReloading || currentAmmo >= maxAmmo) return;

        source.PlayOneShot(gunInfo.reloadClip);
        reloadCoroutine = StartCoroutine(nameof(Recharge));
        anim.SetTrigger("Reload");

    }

    private void noAmmo()
    {
        source.PlayOneShot(gunInfo.emptyShot);
        canUse = false;
    }

    IEnumerator Recharge()
    {
        isReloading = true;
        canUse = false;
        yield return new WaitForSeconds(reloadTimeDelay);
        currentAmmo = maxAmmo;
        canUse = true;
        isReloading = false;
        ammoUI.updateAmmoTxt(currentAmmo, maxAmmo);
    }
    void impactPool() => pv.RPC("RPC_ImpactPool", RpcTarget.All);


    [PunRPC]
    void RPC_Shoot(Vector3 hitPos, Vector3 hitNormal)
    {
        // play sound online
        source.PlayOneShot(gunInfo.useClip);

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
