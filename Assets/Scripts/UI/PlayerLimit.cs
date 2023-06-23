using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLimit : MonoBehaviour
{
    public static PlayerLimit Instance;
    public int limit;
    [SerializeField] Slider playerLimitSlider;
    [SerializeField] TextMeshProUGUI playerLimitTxt;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        onSliderUpdate();
    }


    public void onSliderUpdate()
    {
        playerLimitTxt.text = playerLimitSlider.value.ToString();
        limit = (int)playerLimitSlider.value;
    }
}
