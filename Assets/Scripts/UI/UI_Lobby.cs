using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [Header("���")]
    public Toggle ToggleGameMenu;
    public Toggle ToggleShopMenu;
    public Toggle TogglePlayerMenu;

    [Header("Ȱ��ȭ �޴���")]
    public GameObject ObjGameMenu;
    public GameObject ObjShopMenu;
    public GameObject ObjPlayerMenu;

    private void Awake()
    {
        // �ݹ� �����ϱ� ���� ���ִ����� ����.
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
