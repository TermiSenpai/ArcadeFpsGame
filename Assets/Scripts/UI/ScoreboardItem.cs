using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameTxt;
    public TMP_Text killsTxt;
    public TMP_Text deathsTxt;
    public TMP_Text pingTxt;

    Player player;

    public void Initialize(Player player)
    {
        this.player = player;

        usernameTxt.text = player.NickName;
        UpdateStats();
    }

    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsTxt.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathsTxt.text = deaths.ToString();
        }
        if(player.CustomProperties.TryGetValue("ping", out object ping))
        {
            pingTxt.text = ping.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hastable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths") || changedProps.ContainsKey("ping"))
                UpdateStats();
        }
    }

}
