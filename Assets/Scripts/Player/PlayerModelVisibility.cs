using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelVisibility : MonoBehaviour
{
    [SerializeField] GameObject playerModel;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void disableModel() => playerModel.SetActive(false);
    public void enableModel() => playerModel.SetActive(true);

    private void OnEnable()
    {
        playerModel.SetActive(true);
    }

}
