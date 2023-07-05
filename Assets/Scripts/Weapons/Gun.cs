using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : Item
{
    public GameObject bulletImpactPrefab;
    public abstract override void Use();
}
