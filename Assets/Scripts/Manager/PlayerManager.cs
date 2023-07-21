using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hastable = ExitGames.Client.Photon.Hashtable;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;
    GameObject player;
    GameObject skullEffect;
    GameObject explosionEffect;

    const string path = "PhothonPrefabs";

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

        if (player == null)
            player = PhotonNetwork.Instantiate(Path.Combine(path, "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { pv.ViewID });

        player.SetActive(true);
        player.transform.position = spawnpoint.position;

        // set gameobjet name in editor, just for debug
#if UNITY_EDITOR
        player.name = PhotonNetwork.NickName;
#endif
    }

    public void die()
    {
        showDeathEffect();
        Respawn();
        deaths++;
        SendHash("deaths", deaths);
    }

    private void Respawn()
    {
        player.SetActive(false);
        //PhotonNetwork.Destroy(player);
        //Invoke(nameof(createController), 2.5f);
        createController();
    }

    void showDeathEffect()
    {
        if (skullEffect == null)
            skullEffect = PhotonNetwork.Instantiate(Path.Combine(path, "Skull"), player.transform.position + Vector3.up, Quaternion.identity, 0, new object[] { pv.ViewID });
        
        if (explosionEffect == null)
            explosionEffect = PhotonNetwork.Instantiate(Path.Combine(path, "Explosion"), player.transform.position, Quaternion.Euler(-90f, 0, 0), 0, new object[] { pv.ViewID });

        skullEffect.transform.position = player.transform.position + Vector3.up;
        explosionEffect.transform.position = player.transform.position;

        skullEffect.SetActive(true);
        explosionEffect.SetActive(true);
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
