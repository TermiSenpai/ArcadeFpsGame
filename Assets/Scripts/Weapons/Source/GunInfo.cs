using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FPS/New gun")]
public class GunInfo : ItemInfo
{
    public float damage;
    public float maxDistance;

    public AudioClip useClip;
    public AudioClip reloadClip;
    public AudioClip aimClip;
    public AudioClip emptyShot;
}
