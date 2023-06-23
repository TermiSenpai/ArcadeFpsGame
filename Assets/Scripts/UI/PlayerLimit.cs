using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLimit : MonoBehaviour
{
    [SerializeField] Slider playerLimitSlider;
    [SerializeField] TextMeshProUGUI playerLimitTxt;

    private void Start()
    {
        onSliderUpdate();
    }


    public void onSliderUpdate()
    {
        playerLimitTxt.text = playerLimitSlider.value.ToString();
    }
}
