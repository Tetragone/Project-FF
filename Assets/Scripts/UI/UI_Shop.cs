using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    public Button ButtonGachaFish1;
    public Button ButtonGachaFish10;

    public Button ButtonGachaRelic1;
    public Button ButtonGachaRelic10;

    private void Awake()
    {
        InitButtonCallbacks();
        Refresh();
    }

    private void InitButtonCallbacks()
    {
        ButtonGachaFish1.onClick.AddListener(() =>
        {
            GachaFishes(1);
        });

        ButtonGachaFish10.onClick.AddListener(() =>
        {
            GachaFishes(10);
        });

        ButtonGachaRelic1.onClick.AddListener(() =>
        {
            GachaRelic(1);
        });

        ButtonGachaRelic10.onClick.AddListener(() =>
        {
            GachaRelic(10);
        });
    }

    private void GachaFishes(int count)
    {
        int need = count * GameStaticValue.FishPrice;
        if (!UserDataMgr.Instance.IsEnoughGoods(need, GoodsType.gacha_point))
        {
            return;
        }

        count = count + count / 10 + (count / 100) * 10;
        List<string> selectedFishes = GachaMgr.GachaFish(count);
        UserDataMgr.Instance.UseGoods(need, GoodsType.gacha_point);

        foreach (string fid in selectedFishes)
        {
            UpgradeMgr.Instance.AddFishesCount(fid);
        }

        UpgradeMgr.Instance.SaveData();
        Refresh();
    }

    private void GachaRelic(int count)
    {
        int need = count * GameStaticValue.RelicPrice;
        if (!UserDataMgr.Instance.IsEnoughGoods(need, GoodsType.relic_point))
        {
            return;
        }

        count = count + count / 10;
        List<string> selectedRelics = GachaMgr.GachaRelic(count);
        UserDataMgr.Instance.UseGoods(need, GoodsType.gacha_point);

        foreach(string rid in selectedRelics)
        {
            UpgradeMgr.Instance.AddRelicCount(rid);
        }

        UpgradeMgr.Instance.SaveData();
        Refresh();
    }

    private void Refresh()
    {
        ButtonGachaFish1.interactable = UserDataMgr.Instance.IsEnoughGoods(GameStaticValue.FishPrice, GoodsType.gacha_point);
        ButtonGachaFish10.interactable = UserDataMgr.Instance.IsEnoughGoods(GameStaticValue.FishPrice * 10, GoodsType.gacha_point);
        ButtonGachaFish1.interactable = UserDataMgr.Instance.IsEnoughGoods(GameStaticValue.RelicPrice, GoodsType.relic_point);
        ButtonGachaFish10.interactable = UserDataMgr.Instance.IsEnoughGoods(GameStaticValue.RelicPrice * 10, GoodsType.relic_point);
    }

    private void OnEnable()
    {
        Refresh();
    }
}
