using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraWeapons : MonoBehaviour
{
    [SerializeField] PhotonView playerPv;
    [SerializeField] GameObject root;
    [SerializeField] int weaponLayerValue = 8;

    private void Start()
    {
        if (playerPv.IsMine)
            changeLayer();
    }
    void changeLayer()
    {
        root.layer = weaponLayerValue;
        Transform[] childs = GetComponentsInChildren<Transform>();
        foreach (Transform child in childs)
            child.gameObject.layer = weaponLayerValue;
    }
}
