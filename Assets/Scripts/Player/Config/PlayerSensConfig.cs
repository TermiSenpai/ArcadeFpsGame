using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerSensOptions", menuName = "PlayerConfig/Player Sens Config")]

public class PlayerSensConfig : ScriptableObject
{
    [Range (0.1f, 1f)] public float CamSensY;
    [Range(0.1f, 1f)] public float CamSensX;
}
