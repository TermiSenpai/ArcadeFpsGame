using UnityEngine;

[CreateAssetMenu(fileName ="PlayerMovConfig", menuName ="PlayerConfig/Player Movement config")]
public class PlayerMovConfig : ScriptableObject
{
    [Tooltip("Velocidad frontal")]
    public float fowardSpeed;
    [Tooltip("Velocidad lateral")]
    public float strafeSpeed;
    public float airMultiplier = 5f;
    public float movementMultiplier = 10f;
    public float groundDrag = 6f;
    public float airDrag = 2f;
}
