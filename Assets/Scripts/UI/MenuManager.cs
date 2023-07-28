using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenMenu(string menuName)
    {
        foreach (var menu in menus)
        {
            if (menu.menuName == menuName)
                OpenMenu(menu);
            else if (menu.isOpen)
                CloseMenu(menu);

        }
    }

    public void OpenMenu(Menu menu)
    {
        CheckOpenMenus();
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    private void CheckOpenMenus()
    {
        foreach (var menu in menus)
        {
            if (menu.isOpen)
                CloseMenu(menu);
        }
    }
}
