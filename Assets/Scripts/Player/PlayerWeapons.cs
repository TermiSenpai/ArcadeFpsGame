using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerWeapons : MonoBehaviourPunCallbacks
{
    #region variables
    [Header("References")]
    [SerializeField] PlayerIngameSettings settings;
    [SerializeField] Item[] items;

    int itemIndex = -1;
    int previusItemIndex = -1;

    public bool isAiming = false;
    public bool canChangeWeapon = true;
    PhotonView pv;

    // Delegate events
    public delegate void OnWeaponChanged();
    public static event OnWeaponChanged OnWeaponChangedRelease;

    #endregion

    #region unity
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!pv.IsMine)
            return;

        EquipItem(0);
        Sync();
    }

    #endregion

    #region custom methods
    public void SetWeaponDefault()
    {
        EquipItem(0);
        items[itemIndex].Default();
    }

    public void EquipItem(int _index)
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

        if (items[0].gameObject.activeInHierarchy)
            OnWeaponChangedRelease?.Invoke();

        Sync();
    }

    private void Sync()
    {
        if (!pv.IsMine) return;

        Hashtable hash = new()
        {
            { "itemIndex", itemIndex }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    private void CheckInputValue(Vector2 value)
    {
        if (value.y > 0)
        {
            if (itemIndex >= items.Length - 1)
                EquipItem(0);
            else
                EquipItem(itemIndex + 1);
        }

        else
        {
            if (itemIndex <= 0)
                EquipItem(items.Length - 1);
            else
                EquipItem(itemIndex - 1);

        }
    }

    private void SwitchWeapon()
    {
        switch (itemIndex)
        {
            case 0:
                EquipItem(1);
                break;
            case 1:
                EquipItem(0);
                break;
        }
    }

    #endregion

    #region overrides
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !pv.IsMine && targetPlayer == pv.Owner)
            EquipItem((int)changedProps["itemIndex"]);
    }

    #endregion

    #region Input


    public void OnScrollInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine)
            return;

        if (!canChangeWeapon) return;
        if (settings.GetState() == State.paused) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                CheckInputValue(context.ReadValue<Vector2>());
                break;
        }
    }
    public void OnGamepadSwitchInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine)
            return;
        if (!canChangeWeapon) return;
        if (settings.GetState() == State.paused) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                SwitchWeapon();
                break;
        }
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine)
            return;
        if (settings.GetState() == State.paused) return;

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

        if (settings.GetState() == State.paused) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                items[itemIndex].Aim();
                isAiming = true;
                canChangeWeapon = false;
                break;
            case InputActionPhase.Canceled:
                items[itemIndex].StopAim();
                isAiming = false;
                canChangeWeapon = true;
                break;
        }
    }

    public void OnReloadInput(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        if (settings.GetState() == State.paused) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                items[itemIndex].Reload();
                break;
        }
    }

    #endregion
}
