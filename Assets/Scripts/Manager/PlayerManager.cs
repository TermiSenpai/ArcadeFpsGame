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
        Transform spawnpoint = SpawnpointManager.Instance.GetRandomSpawnPoint();
        player = PhotonNetwork.Instantiate(Path.Combine("PhothonPrefabs", "Player"), spawnpoint.position, Quaternion.identity, 0, new object[] { pv.ViewID });

        // set gameobjet name in editor, just for debug
        player.name = PhotonNetwork.NickName;
    }

    public void die()
    {
        PhotonNetwork.Destroy(player);
        createController();
    }
}
