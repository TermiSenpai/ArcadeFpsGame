using UnityEngine;

[CreateAssetMenu(fileName ="PlayerMovConfig", menuName ="PlayerConfig/Player Movement config")]
public class PlayerMovConfig : ScriptableObject
{
    public float moveSpeed;
    public float movMultiplier;
    public float airMovMultiplier;
    public float groundDrag;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
}
