using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] PlayerSensConfig sensConfig;
    [SerializeField] Slider xSlider;
    [SerializeField] Slider ySlider;

    private void Start()
    {
        xSlider.value = PlayerPrefs.GetFloat("sensX");
        ySlider.value = PlayerPrefs.GetFloat("sensY");
        
    }

    public void setXSens(float newValue) => sensConfig.CamSensX = newValue;
    public void setYSens(float newValue) => sensConfig.CamSensY = newValue;

    private void OnEnable()
    {
        xSlider.value = sensConfig.CamSensX;
        ySlider.value = sensConfig.CamSensY;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("sensX", xSlider.value);
        PlayerPrefs.SetFloat("sensY", ySlider.value);
    }


}
