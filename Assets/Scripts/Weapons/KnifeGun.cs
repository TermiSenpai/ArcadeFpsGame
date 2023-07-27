using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class KnifeGun : Gun
{
    int numberOfRays = 5;
    float angleSpread = 30f;

    AudioSource source;


    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] Transform knifeDetection;
    PhotonView pv;
    Animator anim;
    GameObject impact;

    RaycastHit hit;

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
        if (hitDettect())
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            hit.collider.gameObject.GetComponent<IDamageable>()?.takeDamage(gunInfo.damage);
            pv.RPC(nameof(RPC_Shoot), RpcTarget.All, hitPoint, hitNormal);
        }
    }

    private bool hitDettect()
    {
        Vector3 rayDirection = Camera.main.transform.forward;

        // Calcular el ángulo entre rayos adyacentes en el abanico
        float angleIncrement = angleSpread / (numberOfRays - 1);

        // Crear varios rayos en abanico alrededor del rayo central
        for (int i = 0; i < numberOfRays; i++)
        {
            // Calcular la dirección del rayo actual dentro del abanico
            Vector3 currentRayDirection = Quaternion.Euler(0f, -angleSpread / 2 + i * angleIncrement, 0f) * rayDirection;

            // Crear el rayo en la dirección actual
            Ray ray = new Ray(Camera.main.transform.position, currentRayDirection);

            // Realizar el Raycast para obtener información del impacto
            if (Physics.Raycast(ray, out hit, gunInfo.maxDistance, otherPlayerLayer))
                return true;
        }

        return false;
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

    private void OnDrawGizmos()
    {
        // Dibujar el gizmo del raycast central
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * gunInfo.maxDistance);

        // Dibujar rayos adicionales en abanico para representar el rango de detección extendido
        Vector3 rayDirection = Camera.main.transform.forward;
        float angleIncrement = angleSpread / (numberOfRays - 1);
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 currentRayDirection = Quaternion.Euler(0f, -angleSpread / 2 + i * angleIncrement, 0f) * rayDirection;
            Ray fanRay = new Ray(Camera.main.transform.position, currentRayDirection);
            Gizmos.DrawRay(fanRay.origin, fanRay.direction * gunInfo.maxDistance);
        }

        // Dibujar una esfera para representar la posición del inicio del raycast central
        Gizmos.DrawSphere(ray.origin, 0.1f);
    }
}
