using UnityEngine;

[CreateAssetMenu (fileName = "PlayerSensOptions", menuName = "PlayerConfig/Player Sens Config")]

public class PlayerSensConfig : ScriptableObject
{
    public float sensitivity;   
    public float smoothSpeed;

    public int maxY = 90;
    public int minY = -85;
}
