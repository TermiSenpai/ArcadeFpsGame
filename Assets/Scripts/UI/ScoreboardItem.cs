using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    public TMP_Text usernameTxt;
    public TMP_Text killsTxt;
    public TMP_Text deathsTxt;

    public void initialize(Player player)
    {
        usernameTxt.text = player.NickName;
    }

}
