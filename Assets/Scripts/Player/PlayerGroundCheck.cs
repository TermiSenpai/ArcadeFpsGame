using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] PlayerJump playerJump;
    [SerializeField] PhotonView pv;

    private void OnTriggerEnter(Collider other)
    {
        if (pv.IsMine)
            if (other.gameObject.layer == 3) playerJump.setGroundState(true);
    }

}
