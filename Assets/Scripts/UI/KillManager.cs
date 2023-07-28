using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class KillManager : MonoBehaviourPunCallbacks
{
    public static KillManager Instance;

    #region Variables
    [SerializeField] GameObject[] Infos;

    int index = -1;
    KillerInfo info;
    GameObject lastKill;

    #endregion

    #region Unity
    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Custom
    public void EnableKillInfo(string killer, string killed)
    {
        IncreaseIndex();

        // Obtenemos el índice del GameObject actual y lo sincronizamos como una propiedad personalizada
        SendHash("CurrentKillInfoIndex", index);

        Dictionary<string, string> infoDict = new()
        {
            { "Killer", killer },
            { "Killed", killed }
        };

        SendHash("Info", infoDict);


        lastKill = Infos[index];
        lastKill.SetActive(true);
        SetupInfo(killer, killed);
    }

    public void SetupInfo(string killer, string killed)
    {
        info = Infos[index].GetComponent<KillerInfo>();
        info.SetUp(killer, killed);
    }

    void IncreaseIndex()
    {
        index++;

        if (index >= Infos.Length)
            index = 0;
    }

    #endregion

    #region Sync
    void SendHash(string type, int value)
    {
        Hastable hash = new()
        {
            { type, value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void SendHash(string type, Dictionary<string, string> value)
    {
        Hastable hash = new()
        {
            { type, value }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hastable changedProps)
    {

        // Verificamos si la propiedad personalizada "CurrentKillInfoIndex" ha cambiado para sincronizar el estado del objeto "KillInfo"
        if (changedProps.ContainsKey("CurrentKillInfoIndex"))
        {
            index = (int)changedProps["CurrentKillInfoIndex"];
            lastKill = Infos[index];
            lastKill.SetActive(true);
        }

        if (changedProps.ContainsKey("Info"))
        {
            // Obtener el diccionario "Info" de las propiedades personalizadas
            if (changedProps["Info"] is Dictionary<string, string> infoDict)
            {
                // Obtener los valores de "Killer" y "Killed" del diccionario "Info"
                if (infoDict.TryGetValue("Killer", out string killer) && infoDict.TryGetValue("Killed", out string killed))
                {
                    SetupInfo(killer, killed);
                }
            }
        }


    }

    #endregion
}
