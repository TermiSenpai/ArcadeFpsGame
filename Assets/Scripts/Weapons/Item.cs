using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public GameObject itemGameobject;
    public GameObject handsGameobject;

    public abstract void Use();
    public abstract void Aim();
    public abstract void StopAim();
}