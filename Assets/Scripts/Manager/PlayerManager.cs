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
#if UNITY_EDITOR
        player.name = PhotonNetwork.NickName;
#endif
    }

    public void die()
    {
        PhotonNetwork.Destroy(player);
        createController();

        deaths++;
        SendHash("deaths", deaths);
    }

    public void getKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;
        SendHash("kills", kills);
    }

    void SendHash(string type, int value)
    {

        Hastable hash = new Hastable();
        hash.Add(type, value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pv.Owner == player);
    }
}
