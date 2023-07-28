using Photon.Pun;
using TMPro;
using UnityEngine;

public class ProfileName : MonoBehaviour
{
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_Text placeholder;


    private void OnEnable()
    {
        placeholder.text = PhotonNetwork.NickName;
        nickname.text = string.Empty;
    }

    public void SaveNickname()
    {
        string newNickname = nickname.text;
        if (newNickname != string.Empty)
        {
            PlayerPrefs.SetString("Nickname", newNickname);
            PhotonNetwork.NickName = newNickname;
        }
    }
}
