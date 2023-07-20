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

    public void openMenu(string menuName)
    {
        foreach (var menu in menus)
        {
            if (menu.menuName == menuName)
                openMenu(menu);
            else if (menu.isOpen)
                closeMenu(menu);

        }
    }

    public void openMenu(Menu menu)
    {
        checkOpenMenus();
        menu.open();
    }

    public void closeMenu(Menu menu)
    {
        menu.close();
    }

    private void checkOpenMenus()
    {
        foreach (var menu in menus)
        {
            if (menu.isOpen)
                closeMenu(menu);
        }
    }
}
