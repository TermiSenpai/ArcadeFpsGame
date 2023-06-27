using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerMovConfig", menuName ="PlayerConfig/Player Movement config")]
public class PlayerMovConfig : ScriptableObject
{
    [Tooltip("Velocidad frontal")]
    public float fowardSpeed;
    [Tooltip("Velocidad lateral")]
    public float strafeSpeed;
    public float movementMultiplier = 10f;
    public float rbDrag = 6f;
}
