using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;
    GameObject player;

    int kills = 0;
    int deaths = 0;

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
        player = PhotonNetwork.Instantiate(Path.Combine("PhothonPrefabs", "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { pv.ViewID });

        // set gameobjet name in editor, just for debug
        player.name = PhotonNetwork.NickName;
    }

    public void die()
    {        
        PhotonNetwork.Destroy(player);
        createController();

        deaths++;

        Hastable hash = new Hastable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void getKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hastable hash = new Hastable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pv.Owner == player);
    }
}
