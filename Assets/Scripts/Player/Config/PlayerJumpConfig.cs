using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerJumpConfig", menuName = "PlayerConfig/Player Jump")]
public class PlayerJumpConfig : ScriptableObject
{
    public float gravity;
    public float gravityMultiplier;
    public float jumpForce;
    public LayerMask groundLayer;
    public float checkRadius;
}
