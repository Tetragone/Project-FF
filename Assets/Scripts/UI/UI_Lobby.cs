using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : Singleton<UI_Lobby>
{
    public static GameObject Root;

    [SerializeField] private GameObject _root;

    [Header("토글")]
    public Toggle ToggleUpgradeMenu;
    public Toggle ToggleFishes;
    public Toggle ToggleGameMenu;
    public Toggle ToggleRelic;
    public Toggle ToggleShopMenu;

    public Button ButtonPlay;

    [Header("활성화 메뉴들")]
    public GameObject ObjMenu;
    public GameObject ObjUpgradeMenu;
    public GameObject ObjFishes;
    public GameObject ObjGameMenu;
    public GameObject ObjRelices;
    public GameObject ObjShopMenu;
    public GameObject ObjGamePlayMenu;
    public GameObject ObjGameRaceMenu;

    [Header("Fade Obj")]
    public Image ImageFade;

    [Space(7)]
    public UI_Top Top;

    private Dictionary<MenuType, GameObject> OnOffUIs = new Dictionary<MenuType, GameObject>();

    protected override void Awake()
    {
        base.Awake();

        if (Root == null)
        {
            Root = _root;
        }

        // 콜백 설정하기 전에 해주는편이 좋다.
        ToggleGameMenu.isOn = true;
        ToggleUpgradeMenu.isOn = false;
        ToggleFishes.isOn = false;
        ToggleRelic.isOn = false;
        ToggleShopMenu.isOn = false;
        InitButtonCallback();
        SetOnOffUIs();
        SetActiveMenu(MenuType.game_menu);
        StartFadeOut(0.5f);
    }

    private void SetOnOffUIs()
    {
        OnOffUIs.Add(MenuType.upgrade_menu, ObjUpgradeMenu);
        OnOffUIs.Add(MenuType.fish_menu, ObjFishes);
        OnOffUIs.Add(MenuType.game_menu, ObjGameMenu);
        OnOffUIs.Add(MenuType.relic_menu, ObjRelices);
        OnOffUIs.Add(MenuType.shop_menu, ObjShopMenu);
        OnOffUIs.Add(MenuType.game_play_menu, ObjGamePlayMenu);
        OnOffUIs.Add(MenuType.game_race_menu, ObjGameRaceMenu);
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

        ToggleFishes.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.fish_menu);
            }
        });

        ToggleShopMenu.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.shop_menu);
            }
        });

        ToggleRelic.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                SetActiveMenu(MenuType.relic_menu);
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
        foreach (var ui in OnOffUIs.Values)
        {
            ui.SetActive(false);
        }

        switch (menu)
        {
            case MenuType.game_menu:
            case MenuType.shop_menu:
            case MenuType.upgrade_menu:
            case MenuType.fish_menu:
                ObjMenu.SetActive(true);
                break;
            case MenuType.game_play_menu:
                ObjMenu.SetActive(false);
                AquaMgr.Instance.InitStart();
                break;
            case MenuType.game_race_menu:
                ObjMenu.SetActive(false);
                break;
        }

        OnOffUIs[menu].SetActive(true);
        this.RefreshTexts();
    }

    public void StartFadeIn(float time)
    {
        StartCoroutine(FadeIn(time));
    }

    private IEnumerator FadeIn(float time)
    {
        float timer = 0f;

        while (timer < time)
        {
            float alpha = timer / time;
            timer += Time.deltaTime;
            ImageFade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        ImageFade.color = new Color(0, 0, 0, 1);
        yield return null;
    }

    public void StartFadeOut(float time)
    {
        StartCoroutine(FadeOut(time));
    }

    private IEnumerator FadeOut(float time)
    {
        float timer = 0f;

        while (timer < time)
        {
            float alpha = (time - timer) / time;
            timer += Time.deltaTime;
            ImageFade.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        ImageFade.color = new Color(0, 0, 0, 0);
        yield return null;
    }

    public void RefreshTexts()
    {
        Top.RefreshText();
    }

    public enum MenuType
    {
        game_menu, shop_menu, upgrade_menu, game_play_menu, game_race_menu, fish_menu, relic_menu,
    }
}