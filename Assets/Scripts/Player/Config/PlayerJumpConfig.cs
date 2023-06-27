using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerJumpConfig", menuName ="PlayerConfig/Player Jump")]
public class PlayerJumpConfig : ScriptableObject
{
    public float gravityMultiplier = 2.5f;
    public float jumpForce = 8.5f;
    public LayerMask ground;
}
