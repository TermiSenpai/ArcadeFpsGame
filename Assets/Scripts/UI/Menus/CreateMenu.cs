using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateMenu : Menu
{
    [SerializeField] TMP_Text roomname;
    [SerializeField] TMP_Text roomPlaceholder;


    private void OnDisable()
    {
        roomname.text = string.Empty;
        roomPlaceholder.text = "Enter room name";
    }
}
