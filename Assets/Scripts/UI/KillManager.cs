using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class KillManager : MonoBehaviourPunCallbacks
{
    public static KillManager Instance;

    [SerializeField] GameObject[] Infos;
    PhotonView pv;
    int index = -1;
    KillerInfo info;
    GameObject lastKill;

    private void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
    }

    public void enableKillInfo(string killer, string killed)
    {
        increaseIndex();

        // Obtenemos el índice del GameObject actual y lo sincronizamos como una propiedad personalizada
        SendHash("CurrentKillInfoIndex", index);

        Dictionary<string, string> infoDict = new Dictionary<string, string>();
        infoDict.Add("Killer", killer);
        infoDict.Add("Killed", killed);

        SendHash("Info", infoDict);


        //Hastable hash = new Hastable();
        //hash.Add("Info", info);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        

        lastKill = Infos[index];
        lastKill.SetActive(true);
        setupInfo(killer, killed);
    }

    public void setupInfo(string killer, string killed)
    {
        info = Infos[index].GetComponent<KillerInfo>();
        info.setUp(killer, killed);
    }

    void increaseIndex()
    {
        index++;

        if (index >= Infos.Length)
            index = 0;
    }

    void SendHash(string type, int value)
    {
        Hastable hash = new Hastable();
        hash.Add(type, value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void SendHash(string type, Dictionary<string, string> value)
    {
        Hastable hash = new Hastable();
        hash.Add(type, value);
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
                    setupInfo(killer, killed);
                }
            }
        }


    }
}
