using UnityEngine;

[CreateAssetMenu(fileName ="PlayerMovConfig", menuName ="PlayerConfig/Player Movement config")]
public class PlayerMovConfig : ScriptableObject
{
    public float movSpeed;

    public float acceleration;
    public float maxSpeed;
    public float movMultiplier;
    public float airMovMultiplier;
    public float groundDrag;
    public float airDrag;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
}
