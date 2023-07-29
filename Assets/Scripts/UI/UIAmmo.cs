using TMPro;
using UnityEngine;

public class UIAmmo : MonoBehaviour
{
    [SerializeField] TMP_Text currentAmmoTxt;
    [SerializeField] GameObject ammoUI;
    [SerializeField] GameObject sniper;

    public void UpdateAmmoTxt(int ammo, int maxAmmo)
    {
        currentAmmoTxt.text = $"{ammo} / {maxAmmo}";
    }

    public void ToggleUI()
    {
        bool toggle = sniper.activeInHierarchy;
        ammoUI.SetActive(toggle);
    }

    private void OnEnable()
    {
        PlayerWeapons.OnWeaponChangedRelease += ToggleUI;
    }
    private void OnDisable()
    {
        PlayerWeapons.OnWeaponChangedRelease -= ToggleUI;
    }
}
