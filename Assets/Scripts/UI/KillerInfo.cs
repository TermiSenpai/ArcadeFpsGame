using TMPro;
using UnityEngine;

public class KillerInfo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text killerTxt;
    [SerializeField] private TMP_Text killedTxt;


    private void OnEnable()
    {
        Invoke(nameof(DisableInfo), 5f);
    }

    public void SetUp(string killerName, string killedName)
    {
        killerTxt.text = killerName;
        killedTxt.text = killedName;
    }

    void DisableInfo()
    {
        gameObject.SetActive(false);
    }

}

