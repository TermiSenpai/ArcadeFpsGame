using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    void FixedUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
