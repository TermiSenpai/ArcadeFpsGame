using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraWeapons : MonoBehaviour
{
    PhotonView playerPv;
    [SerializeField] GameObject[] ObjectsToWeaponLayer;
    [SerializeField] int weaponLayerValue = 8;

    private void Awake()
    {
        playerPv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (playerPv.IsMine)
            changeLayer();
    }
    void changeLayer()
    {
        // objects that need to be of a specific layer
        foreach (GameObject part in ObjectsToWeaponLayer)
        {
            part.layer = weaponLayerValue;
        }
    }
}
