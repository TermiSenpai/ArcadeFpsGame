using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
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
        loadSavedValues();
        setValueToTxt();
        addSliderListeners();
    }

    private void loadSavedValues()
    {
        float normalSens = PlayerPrefs.GetFloat(normalName, 0.5f);
        float aimSens = PlayerPrefs.GetFloat(aimName, 0.5f);

        sensConfig.sensitivity = normalSens;
        sensConfig.aimSens = aimSens;

        normalSlider.value = normalSens;
        aimSlider.value = aimSens;
    }

    private void addSliderListeners()
    {
        normalSlider.onValueChanged.AddListener(setNormalSens);
        aimSlider.onValueChanged.AddListener(setAimSens);
    }

    public void setNormalSens(float newValue)
    {
        sensConfig.sensitivity = newValue;
        setValueToTxt();
        PlayerPrefs.SetFloat(normalName, sensConfig.sensitivity);
    }
    public void setAimSens(float newValue)
    {
        sensConfig.aimSens = newValue;
        setValueToTxt();
        PlayerPrefs.SetFloat(aimName, sensConfig.aimSens);
    }

    public void setValueToTxt()
    {
        normalTxt.text = (normalSlider.value * 10).ToString("0.00");
        aimTxt.text = (aimSlider.value * 10).ToString("0.00");
    }


}
