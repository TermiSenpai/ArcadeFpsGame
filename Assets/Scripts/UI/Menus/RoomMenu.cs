using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMenu : Menu
{
    [SerializeField] Transform container;
    private void OnDisable()
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
