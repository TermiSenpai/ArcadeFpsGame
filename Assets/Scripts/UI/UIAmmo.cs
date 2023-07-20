using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAmmo : MonoBehaviour
{
    [SerializeField] TMP_Text currentAmmoTxt;
    [SerializeField] GameObject ammoUI;

    public void updateAmmoTxt(int ammo, int maxAmmo)
    {
        currentAmmoTxt.text = $"{ammo} / {maxAmmo}";
    }

    public void toggleUI(bool toggle)
    {
        ammoUI.SetActive(toggle);
    }
}
