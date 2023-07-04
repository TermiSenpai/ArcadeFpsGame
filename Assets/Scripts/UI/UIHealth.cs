using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] Image healthImage;

    public void updateHealthBar(float health)
    {
        healthImage.fillAmount = health;
    }

}
