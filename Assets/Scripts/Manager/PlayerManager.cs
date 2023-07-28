using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;

    [SerializeField] AudioClip deathClip;

    GameObject player;
    PlayerWeapons pWeapon;
    AudioSource source;

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
            CreateController();
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnpointManager.Instance.GetRandomSpawnPoint();

        if (player == null)
        {
            player = PhotonNetwork.Instantiate(Path.Combine(path, "Player"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { pv.ViewID });
            pWeapon = player.GetComponent<PlayerWeapons>();
            source = player.GetComponent<AudioSource>();
        }

        TogglePlayer(true);
        pWeapon.SetWeaponDefault();

        player.transform.SetPositionAndRotation(spawnpoint.position, spawnpoint.rotation);

        // set gameobjet name in editor, just for debug
#if UNITY_EDITOR
        player.name = PhotonNetwork.NickName;
#endif
    }
    void TogglePlayer(bool value)
    {
        player.SetActive(value);
    }

    public void Die()
    {
        ShowDeathEffect();
        TogglePlayer(false);

        deaths++;
        SendHash("deaths", deaths);
        Respawn();
        source.PlayOneShot(deathClip);
    }

    private void Respawn()
    {
        CreateController();
    }

    void ShowDeathEffect()
    {
        skullEffect = PhotonNetwork.Instantiate(Path.Combine(path, "Skull"), player.transform.position + Vector3.up, Quaternion.identity, 0, new object[] { pv.ViewID });
        explosionEffect = PhotonNetwork.Instantiate(Path.Combine(path, "Explosion"), player.transform.position, Quaternion.Euler(-90f, 0, 0), 0, new object[] { pv.ViewID });
    }

    public void GetKill() => pv.RPC(nameof(RPC_GetKill), pv.Owner);

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;
        SendHash("kills", kills);
    }

    void SendHash(string type, int value)
    {
        Hastable hash = new()
        {
            { type, value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pv.Owner == player);
    }

    public string GetNickname()
    {
        return PhotonNetwork.NickName;
    }

}
