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
        playerLimitSlider.onValueChanged.AddListener(OnSliderUpdate);
        OnSliderUpdate(2);
    }


    public void OnSliderUpdate(float value)
    {
        playerLimitTxt.text = playerLimitSlider.value.ToString();
        limit = (int)playerLimitSlider.value;
    }
}
