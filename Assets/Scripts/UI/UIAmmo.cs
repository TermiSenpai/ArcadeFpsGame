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

    public void toggleUI()
    {
        bool toggle = !ammoUI.activeInHierarchy;
        ammoUI.SetActive(toggle);
    }

    private void OnEnable()
    {
        PlayerWeapons.OnWeaponChangedRelease += toggleUI;
    }
    private void OnDisable()
    {
        PlayerWeapons.OnWeaponChangedRelease -= toggleUI;
    }
}
