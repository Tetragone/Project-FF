using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : Singleton<UI_Lobby>
{
    public static GameObject Root;

    [SerializeField] private GameObject _root;

    [Header("토글")]
    public Toggle ToggleUpgradeMenu;
    public Toggle ToggleGameMenu;
    public Toggle ToggleShopMenu;

    public Button ButtonPlay;

    [Header("활성화 메뉴들")]
    public GameObject ObjMenu;
    public GameObject ObjUpgradeMenu;
    public GameObject ObjGameMenu;
    public GameObject ObjShopMenu;
    public GameObject ObjGamePlayMenu;
    public GameObject ObjGameRaceMenu;

    protected override void Awake()
    {
        base.Awake();

        if (Root == null)
        {
            Root = _root;
        }

        // 콜백 설정하기 전에 해주는편이 좋다.
        ToggleGameMenu.isOn = true;
        ToggleShopMenu.isOn = false;
        ToggleUpgradeMenu.isOn = false;
        InitButtonCallback();
        SetActiveMenu(MenuType.game_menu);
    }

    private void InitButtonCallback()
    {
        ToggleGameMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn) 
            {
                SetActiveMenu(MenuType.game_menu);
            }
        });

        ToggleShopMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.shop_menu);
            }
        });

        ToggleUpgradeMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.upgrade_menu);
            }
        });

        ButtonPlay.onClick.AddListener(() =>
        {
            SetActiveMenu(MenuType.game_play_menu);
        });
    }

    public void SetActiveMenu(MenuType menu)
    {
        switch (menu)
        {
            case MenuType.game_menu:
                ObjGameMenu.SetActive(true);
                ObjShopMenu.SetActive(false);
                ObjUpgradeMenu.SetActive(false);
                ObjGamePlayMenu.SetActive(false);
                ObjMenu.SetActive(true);
                ObjGameRaceMenu.SetActive(false);
                break;
            case MenuType.shop_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(true);
                ObjUpgradeMenu.SetActive(false);
                ObjGamePlayMenu.SetActive(false);
                ObjMenu.SetActive(true);
                ObjGameRaceMenu.SetActive(false);
                break;
            case MenuType.upgrade_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(false);
                ObjUpgradeMenu.SetActive(true);
                ObjGamePlayMenu.SetActive(true);
                ObjMenu.SetActive(true);
                ObjGameRaceMenu.SetActive(false);
                break;
            case MenuType.game_play_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(false);
                ObjUpgradeMenu.SetActive(false);
                ObjGamePlayMenu.SetActive(true);
                ObjMenu.SetActive(false);
                ObjGameRaceMenu.SetActive(false);
                AquaMgr.Instance.InitStart();
                break;
            case MenuType.game_race_menu:
                ObjGameMenu.SetActive(false);
                ObjShopMenu.SetActive(false);
                ObjUpgradeMenu.SetActive(false);
                ObjGamePlayMenu.SetActive(false);
                ObjMenu.SetActive(false);
                ObjGameRaceMenu.SetActive(true);
                break;
        }
    }

    public enum MenuType
    {
        game_menu, shop_menu, upgrade_menu, game_play_menu, game_race_menu,
    }
}