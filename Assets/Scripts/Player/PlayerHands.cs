using Photon.Pun;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    PhotonView pv;
    [SerializeField] GameObject hands;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!pv.IsMine)
        {
            hands.SetActive(false);
        }
    }

}
