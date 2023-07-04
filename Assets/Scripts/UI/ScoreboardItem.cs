using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameTxt;
    public TMP_Text killsTxt;
    public TMP_Text deathsTxt;

    Player player;

    public void initialize(Player player)
    {
        this.player = player;

        usernameTxt.text = player.NickName;
        updateStats();
    }

    void updateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsTxt.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathsTxt.text = deaths.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hastable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
                updateStats();
        }
    }

}
