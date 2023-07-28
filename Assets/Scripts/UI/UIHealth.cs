using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] Image healthImage;
    [SerializeField] TMP_Text currentHealthTxt;

    public void UpdateHealthBar(float health)
    {
        healthImage.fillAmount = health;
    }

    public void UpdateHealthTxt(string health)
    {
        currentHealthTxt.text = health;
    }

}
