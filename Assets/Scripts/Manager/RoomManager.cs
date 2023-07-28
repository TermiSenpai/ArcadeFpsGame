using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region Variables
    public static RoomManager Instance;
    private PhotonView pv;

    #endregion

    #region Unity
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
     pv = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion

    #region Network
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) // in the game scene
        {
            PhotonNetwork.Instantiate(Path.Combine("PhothonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
    
    public void LoadLevel()
    {
        pv.RPC(nameof(RPC_LoadLevel), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadLevel()
    {
        PhotonNetwork.LoadLevel(1);
    }
    #endregion
}
