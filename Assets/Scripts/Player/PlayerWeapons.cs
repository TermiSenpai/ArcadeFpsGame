using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] Item[] items;
    int itemIndex;
    int previusItemIndex = -1;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!pv.IsMine)
            return;

        equipItem(0);
    }

    void equipItem(int _index)
    {
        if (_index == itemIndex) return;

        itemIndex = _index;

        items[itemIndex].itemGameobject.SetActive(true);

        if (previusItemIndex != -1)
            items[previusItemIndex].itemGameobject.SetActive(false);

        previusItemIndex = itemIndex;
    }



    #region Input

    private void checkInputValue(Vector2 value)
    {

        if (value.y > 0)
        {
            if (itemIndex >= items.Length - 1)
                equipItem(0);
            else
                equipItem(itemIndex + 1);
        }

        else
        {
            if (itemIndex <= 0)
                equipItem(items.Length - 1);
            else
                equipItem(itemIndex - 1);

        }

    }

    public void OnScrollInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                checkInputValue(context.ReadValue<Vector2>());
                break;
        }
    }

    #endregion
}
