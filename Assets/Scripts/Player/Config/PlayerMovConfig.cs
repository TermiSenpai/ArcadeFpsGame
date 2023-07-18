using UnityEngine;

[CreateAssetMenu(fileName ="PlayerMovConfig", menuName ="PlayerConfig/Player Movement config")]
public class PlayerMovConfig : ScriptableObject
{ 
    public float maxSpeed;
    [Header("Velocidad")]
    public float groundSpeed;
    public float airSpeed;
    [Header("Fisica")]
    public float acceleration;
    public float groundDeceleration;
    public float airDeceleration;
    public float grapplingDeceleration;
    [Header("SFX")]
    public AudioClip walkClip;
    public AudioClip deathClip;
}
