using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] PlayerSensConfig sensConfig;
    [SerializeField] Slider normalSlider;
    [SerializeField] Slider aimSlider;
    [SerializeField] TMP_Text normalTxt;
    [SerializeField] TMP_Text aimTxt;

    private void Start()
    {
        sensConfig.sensitivity = PlayerPrefs.GetFloat("normalSens");
        sensConfig.aimSens = PlayerPrefs.GetFloat("aimSens");
    }

    public void setNormalSens(float newValue) => sensConfig.sensitivity = newValue;
    public void setAimSens(float newValue) => sensConfig.aimSens = newValue;

    public void setValueToTxt()
    {
        normalTxt.text = (normalSlider.value * 10).ToString("0.00");
        aimTxt.text = (aimSlider.value * 10).ToString("0.00");
    }

    private void OnEnable()
    {
        normalSlider.value = sensConfig.sensitivity;
        aimSlider.value = sensConfig.aimSens;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("normalSens", normalSlider.value);
        PlayerPrefs.SetFloat("aimSens", aimSlider.value);
    }


}
