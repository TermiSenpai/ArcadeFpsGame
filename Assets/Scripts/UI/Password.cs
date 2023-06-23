using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Password : MonoBehaviour
{
    TMP_InputField passwordInput;

    private void Start()
    {
        passwordInput = gameObject.GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
    }
}
