using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] TMP_Text currentHealthTxt;

    public void updateHealthBar(float health)
    {
        healthImage.fillAmount = health;
    }

    public void updateHealthTxt(string health)
    {
        currentHealthTxt.text = health;
    }

}
