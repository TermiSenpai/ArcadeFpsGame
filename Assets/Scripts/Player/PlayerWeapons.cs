using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerWeapons : MonoBehaviourPunCallbacks
{
    [SerializeField] Item[] items;
    int itemIndex = -1;
    int previusItemIndex = -1;

    public bool isAiming = false;
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
        sync();
    }

    void equipItem(int _index)
    {
        if (_index == itemIndex) return;

        itemIndex = _index;


        items[itemIndex].itemGameobject.SetActive(true);
        if (pv.IsMine)
            items[itemIndex].handsGameobject.SetActive(true);

        if (previusItemIndex != -1)
        {
            items[previusItemIndex].itemGameobject.SetActive(false);
            if (pv.IsMine)
                items[previusItemIndex].handsGameobject.SetActive(false);
        }

        previusItemIndex = itemIndex;

        sync();
    }

    private void sync()
    {
        if (!pv.IsMine) return;

        Hashtable hash = new Hashtable
        {
            { "itemIndex", itemIndex }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !pv.IsMine && targetPlayer == pv.Owner)
            equipItem((int)changedProps["itemIndex"]);
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
        if (!pv.IsMine)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                checkInputValue(context.ReadValue<Vector2>());
                break;
        }
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                items[itemIndex].Use();
                break;
        }
    }
    public void OnAimInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine)
            return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                items[itemIndex].Aim();
                isAiming = true;
                break;
            case InputActionPhase.Canceled:
                items[itemIndex].StopAim();
                isAiming = false;
                break;
        }
    }

    #endregion
}
