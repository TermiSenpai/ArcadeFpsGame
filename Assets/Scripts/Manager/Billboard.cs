using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    

    private void Update()
    {
        if (cam == null)
            cam = FindObjectOfType<Camera>();

        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
