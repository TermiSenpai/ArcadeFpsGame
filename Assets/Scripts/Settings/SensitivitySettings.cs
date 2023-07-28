using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] PlayerSensConfig sensConfig;
    [Header("Sliders")]
    [SerializeField] Slider normalSlider;
    [SerializeField] Slider aimSlider;
    [Header("Texts")]
    [SerializeField] TMP_Text normalTxt;
    [SerializeField] TMP_Text aimTxt;

    private const string normalName = "normalSens";
    private const string aimName = "aimSens";

    private void Start()
    {
        LoadSavedValues();
        SetValueToTxt();
        AddSliderListeners();
    }

    private void LoadSavedValues()
    {
        float normalSens = PlayerPrefs.GetFloat(normalName, 0.5f);
        float aimSens = PlayerPrefs.GetFloat(aimName, 0.5f);

        sensConfig.sensitivity = normalSens;
        sensConfig.aimSens = aimSens;

        normalSlider.value = normalSens;
        aimSlider.value = aimSens;
    }

    private void AddSliderListeners()
    {
        normalSlider.onValueChanged.AddListener(SetNormalSens);
        aimSlider.onValueChanged.AddListener(SetAimSens);
    }

    public void SetNormalSens(float newValue)
    {
        sensConfig.sensitivity = newValue;
        SetValueToTxt();
        PlayerPrefs.SetFloat(normalName, sensConfig.sensitivity);
    }
    public void SetAimSens(float newValue)
    {
        sensConfig.aimSens = newValue;
        SetValueToTxt();
        PlayerPrefs.SetFloat(aimName, sensConfig.aimSens);
    }

    public void SetValueToTxt()
    {
        normalTxt.text = (normalSlider.value * 10).ToString("0.00");
        aimTxt.text = (aimSlider.value * 10).ToString("0.00");
    }


}
