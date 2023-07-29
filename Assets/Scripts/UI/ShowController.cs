using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowController : MonoBehaviour
{
    [SerializeField] GameObject keyboardImg;
    [SerializeField] GameObject gamepadImg;

    [SerializeField] bool keyboardImgActive;
    [SerializeField] bool gamepadImgActive;

    public void ToggleImg()
    {
        keyboardImg.SetActive(keyboardImgActive);
        gamepadImg.SetActive(gamepadImgActive);
    }

}
