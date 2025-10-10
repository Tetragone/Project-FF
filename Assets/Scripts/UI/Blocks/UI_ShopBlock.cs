using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopBlock : MonoBehaviour
{
    public TextMeshProUGUI TextTitle;
    public Image ImagePriceType;
    public TextMeshProUGUI TextPrice;
    public Image ImageReward;
    public TextMeshProUGUI TextCount;
    public Button ButtonGet;
    public GameObject ObjNo;

    [HideInInspector]
    public UI_Shop Parent;

    private string Sid;
    private string PriceType = "";
    private int Price;
    private string GetType1 = "";
    private int GetValue1 = 0;

    public void SetIcon(string sid)
    {
        Sid = sid;
        string sadded = TableMgr.GetTableString("shop", Sid, "add");
        int added = sadded != "x" ? int.Parse(sadded) : 0;

        TextTitle.text = string.Format(TransMgr.GetText(TableMgr.GetTableString("shop", Sid, "t_name")), added);
        TextCount.gameObject.SetActive(added > 1);
        TextCount.text = string.Format("x{0}", added);
        PriceType = TableMgr.GetTableString("shop", Sid, "price_type");
        ImagePriceType.sprite = AtlasMgr.Instance.GetUISprite(TableMgr.GetTableString("item", PriceType, "res"));
        Price = TableMgr.GetTableInt("shop", Sid, "price");
        TextPrice.text = Price.ToString();
        GetType1 = TableMgr.GetTableString("shop", Sid, "get_type_1");
        GetValue1 = TableMgr.GetTableInt("shop", Sid, "get_value_1");
        ImageReward.sprite = AtlasMgr.Instance.GetCommonSprite(TableMgr.GetTableString("shop", Sid, "res"));

        ButtonGet.onClick.AddListener(() =>
        {
            GetItems();
        });

        RefreshInteract();
    }

    private void GetItems()
    {
        if (Enum.TryParse(PriceType, false, out GoodsType type))
        {
            if (UserDataMgr.Instance.IsEnoughGoods(Price, type))
            { 
                UserDataMgr.Instance.UseGoods(Price, type);
            }

        }
        else if (PriceType == "cash")
        {

        }
        else if (PriceType == "ad")
        {

        }

        if (Enum.TryParse(GetType1, false, out GoodsType goods))
        {
            UserDataMgr.Instance.AddGoods(GetValue1, goods);
        }
        else
        {
            if (GetType1 == "fish_gacha")
            {
                List<string> selectedFishes = GachaMgr.GachaFish(GetValue1);

                foreach (string fid in selectedFishes)
                {
                    UpgradeMgr.Instance.AddFishesCount(fid);
                }

                UpgradeMgr.Instance.SaveData();
            }
            else if (GetType1 == "relic_gacha")
            {
                List<string> selectedRelics = GachaMgr.GachaRelic(GetValue1);

                foreach (string rid in selectedRelics)
                {
                    UpgradeMgr.Instance.AddRelicCount(rid);
                }

                UpgradeMgr.Instance.SaveData();
            }
        }

        Parent.RefreshAllIcon();
    }

    public void RefreshInteract()
    {
        GoodsType type = (GoodsType)Enum.Parse(typeof(GoodsType), PriceType);
        bool IsEnable = UserDataMgr.Instance.IsEnoughGoods(Price, type);
        ButtonGet.interactable = IsEnable;
        ObjNo.SetActive(!IsEnable);
    }
}
