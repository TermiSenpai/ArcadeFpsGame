using Cinemachine;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SniperGun : Gun
{
    #region variables

    [Header("References")]
    [SerializeField] GameObject scopeOverlay;
    [SerializeField] GameObject crosshairOverlay;
    [SerializeField] UIAmmo ammoUI;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] GameObject weaponCam;
    PhotonView pv;
    Animator anim;
    AudioSource source;
    GameObject impact;

    [Header("Sniper variables")]
    [SerializeField] float timeToScope;
    [SerializeField] int maxAmmo = 3;
    [SerializeField] float reloadTimeDelay;
    int currentAmmo;
    bool isReloading = false;


    Coroutine aimCoroutine;
    Coroutine reloadCoroutine;

    private event Action OnReloadFinished;

    #endregion

    #region unity
    private void OnEnable()
    {
        OnReloadFinished += OnFinishedReload;
    }

    private void OnDisable()
    {
        OnReloadFinished -= OnFinishedReload;
    }

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

    #endregion

    #region overrides
    public override void Use()
    {
        if (currentAmmo == 0) NoAmmo();
        if (!canUse) return;


        Shoot();
    }

    public override void Aim()
    {
        source.PlayOneShot(gunInfo.aimClip);
        anim.SetBool("Scoped", true);
        aimCoroutine = StartCoroutine(nameof(EnableScope));
    }

    public override void StopAim()
    {
        if (aimCoroutine != null)
            StopCoroutine(aimCoroutine);

        anim.SetBool("Scoped", false);
        DisableScopeOverlay();
    }

    public override void Default()
    {
        currentAmmo = maxAmmo;
        canUse = true;
        DisableScopeOverlay();
        OnFinishedReload();
    }

    public override void Reload()
    {
        if (isReloading || currentAmmo >= maxAmmo) return;

        source.PlayOneShot(gunInfo.reloadClip);
        reloadCoroutine = StartCoroutine(nameof(Recharge));
        anim.SetTrigger("Reload");

    }

    #endregion

    #region custom methods
    public void EnableScopeOverlay()
    {
        cam.m_Lens.FieldOfView = 15f;
        scopeOverlay.SetActive(true);
        crosshairOverlay.SetActive(false);
        weaponCam.SetActive(false);
    }
    public void DisableScopeOverlay()
    {
        cam.m_Lens.FieldOfView = 60f;
        scopeOverlay.SetActive(false);
        crosshairOverlay.SetActive(true);
        weaponCam.SetActive(true);
    }

    private void Shoot()
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
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(gunInfo.damage);
            weaponCoroutine = StartCoroutine(WeaponCooldown());
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
        currentAmmo--;
        ammoUI.UpdateAmmoTxt(currentAmmo, maxAmmo);
    }

    private void NoAmmo()
    {
        source.PlayOneShot(gunInfo.emptyShot);
        canUse = false;
    }

    private void OnFinishedReload()
    {
        currentAmmo = maxAmmo;
        canUse = true;
        isReloading = false;
        ammoUI.UpdateAmmoTxt(currentAmmo, maxAmmo);
    }

    #endregion

    #region enumerators

    private IEnumerator EnableScope()
    {
        yield return new WaitForSeconds(timeToScope);
        EnableScopeOverlay();
    }

    private IEnumerator Recharge()
    {
        isReloading = true;
        canUse = false;
        yield return new WaitForSeconds(reloadTimeDelay);
        OnReloadFinished?.Invoke();
    }
    #endregion

    #region RPC

    [PunRPC]
    void RPC_Shoot(Vector3 hitPos, Vector3 hitNormal)
    {
        // play sound online
        source.PlayOneShot(gunInfo.useClip);

        // Define un array para almacenar los colliders detectados por OverlapSphereNonAlloc.
        int maxColliders = 10; // Elige un número adecuado para el máximo de colisionadores esperados.
        Collider[] colliders = new Collider[maxColliders];

        // Realiza la detección de colisiones sin asignar memoria adicional.
        int numColliders = Physics.OverlapSphereNonAlloc(hitPos, 0.3f, colliders);

        // Comprueba si se han detectado colisiones.
        if (numColliders != 0)
        {
            if (impact == null)
                impact = Instantiate(bulletImpactPrefab, ImpactPos(hitPos, hitNormal), ImpactRotation(hitNormal));

            impact.SetActive(true);

            impact.transform.SetPositionAndRotation(ImpactPos(hitPos, hitNormal), ImpactRotation(hitNormal));
        }
    }

    #endregion
}
