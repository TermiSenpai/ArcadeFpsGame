using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BillboardHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] Image billboardHealth;
    [SerializeField] PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public void UpdateBillboardBar(float health)
    {
        pv.RPC(nameof(RPC_UpdateBar), RpcTarget.All, health);
    }

    [PunRPC]
    private void RPC_UpdateBar(float health)
    {
        billboardHealth.fillAmount = health;
    }
}
