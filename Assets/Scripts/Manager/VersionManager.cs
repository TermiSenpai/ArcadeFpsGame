using TMPro;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI versionTxt;

    private void Start()
    {
        versionTxt.text = Application.version;
    }
}
