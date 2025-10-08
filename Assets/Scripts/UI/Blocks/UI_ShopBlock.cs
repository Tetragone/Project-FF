using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        TextTitle.text = string.Format(TransMgr.GetText(TableMgr.GetTableString("shop", Sid, "name")), added);
        TextCount.gameObject.SetActive(added <= 1);
        TextCount.text = string.Format("x{0}", added);
        PriceType = TableMgr.GetTableString("shop", Sid, "price_type");
        ImagePriceType.sprite = AtlasMgr.Instance.GetUISprite(TableMgr.GetTableString("item", PriceType, "res"));
        Price = TableMgr.GetTableInt("shop", Sid, "price");
        TextPrice.text = Price.ToString();
        GetType1 = TableMgr.GetTableString("shop", Sid, "get_type_1");
        GetValue1 = TableMgr.GetTableInt("shop", Sid, "get_value_1");
        ImageReward.sprite = AtlasMgr.Instance.GetCommonSprite(TableMgr.GetTableString("item", GetType1, "res"));

        ButtonGet.onClick.AddListener(() =>
        {

        });
    }

    public void RefreshInteract()
    {

    }
}
