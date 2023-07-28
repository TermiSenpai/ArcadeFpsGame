using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameQualitySettings : MonoBehaviour
{
    [SerializeField] Toggle[] toggles;

    private void Start()
    {
        int lastToggled = PlayerPrefs.GetInt("Quality", 5);
        SetQuality(lastToggled);
        toggles[lastToggled].isOn = true;
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        PlayerPrefs.SetInt("Quality", value);
    }
}
