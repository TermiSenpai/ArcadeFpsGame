using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterModel : MonoBehaviour
{
    PhotonView pv;
    [SerializeField] GameObject[] bodyParts;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(pv.IsMine)
        {
            foreach(GameObject part in bodyParts) 
            {
                part.SetActive(false);
            }
        }
    }
}
