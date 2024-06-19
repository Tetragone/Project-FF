using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [Header("토글")]
    public Toggle ToggleGameMenu;
    public Toggle ToggleShopMenu;
    public Toggle TogglePlayerMenu;

    [Header("활성화 메뉴들")]
    public GameObject ObjGameMenu;
    public GameObject ObjShopMenu;
    public GameObject ObjPlayerMenu;

    private void Awake()
    {
        // 콜백 설정하기 전에 해주는편이 좋다.
        ToggleGameMenu.isOn = true;
        ToggleShopMenu.isOn = false;
        TogglePlayerMenu.isOn = false;
        InitButtonCallback();
        SetActiveMenu(MenuType.main_menu);
    }

    private void InitButtonCallback()
    {
        ToggleGameMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) 
            {
                SetActiveMenu(MenuType.main_menu);
            }
        });

        ToggleShopMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.shop_menu);
            }
        });

        TogglePlayerMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.player_menu);
            }
        });
    }

    private void SetActiveMenu(MenuType menu)
    {
        switch (menu)
        {
            case MenuType.main_menu:
                ObjGameMenu.SetActive(true);
                ObjShopMenu.SetActive(false);
                ObjPlayerMenu.SetActive(false);
                break;
            case MenuType.shop_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(true);
                ObjPlayerMenu.SetActive(false);
                break;
            case MenuType.player_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(false);
                ObjPlayerMenu.SetActive(true);
                break;
        }
    }

    private enum MenuType
    {
        main_menu, shop_menu, player_menu,
    }
}
