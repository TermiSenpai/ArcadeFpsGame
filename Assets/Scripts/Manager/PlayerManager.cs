using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;
    GameObject player;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
            createController();

    }

    void createController()
    {
        player = PhotonNetwork.Instantiate(Path.Combine("PhothonPrefabs", "Player"), SpawnpointManager.Instance.GetRandomSpawnPoint().position, Quaternion.identity, 0, new object[] { pv.ViewID });
        player.name = PhotonNetwork.NickName;
    }

    public void die()
    {
        PhotonNetwork.Destroy(player);
        createController();
    }
}
