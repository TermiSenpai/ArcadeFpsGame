using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileName : MonoBehaviour
{
    [SerializeField] TMP_InputField nickname;
    [SerializeField] TMP_Text placeholder;


    private void OnEnable()
    {
        placeholder.text = PhotonNetwork.NickName;
        nickname.text = string.Empty;
    }

    public void saveNickname()
    {
        string newNickname = nickname.text;
        if (newNickname != string.Empty)
        {
            Debug.Log(newNickname);
            PlayerPrefs.SetString("Nickname", newNickname);
            PhotonNetwork.NickName = newNickname;
        }
    }
}
