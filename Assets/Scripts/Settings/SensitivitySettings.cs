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
        xSlider.value = PlayerPrefs.GetFloat("sens");
        
    }

    public void setSens(float newValue) => sensConfig.sensitivity = newValue;

    private void OnEnable()
    {
        xSlider.value = sensConfig.sensitivity;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("sens", xSlider.value);
    }


}
