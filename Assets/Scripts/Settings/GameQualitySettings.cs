using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameQualitySettings : MonoBehaviour
{
    [SerializeField] Toggle[] toggles;

    private void OnEnable()
    {
        int lastToggled = PlayerPrefs.GetInt("Quality");
        toggles[lastToggled].isOn = true;
    }

    public void setQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        PlayerPrefs.SetInt("Quality", value);
    }
}
