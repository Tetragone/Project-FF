using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public TextMeshProUGUI TextGachaFishTitle;
    public Button ButtonGachaFish1;
    public Button ButtonGachaFish10;

    public TextMeshProUGUI TextGachaRelicTitle;
    public Button ButtonGachaRelic1;
    public Button ButtonGachaRelic10;

    private void Awake()
    {
        InitButtonCallbacks();
    }

    private void InitButtonCallbacks()
    {
        ButtonGachaFish1.onClick.AddListener(() =>
        {

        });

        ButtonGachaFish10.onClick.AddListener(() =>
        {

        });

        ButtonGachaRelic1.onClick.AddListener(() =>
        {

        });

        ButtonGachaRelic10.onClick.AddListener(() =>
        {

        });
    }

    private void GachaFishes(int count)
    {
        List<string> selectedFishes = GachaMgr.GachaFish(count);
        UserDataMgr.Instance.UseGoods(count, GoodsType.gacha_point);
    }
}
