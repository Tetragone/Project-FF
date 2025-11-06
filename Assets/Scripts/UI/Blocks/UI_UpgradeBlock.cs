using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeBlock : MonoBehaviour
{
    public TextMeshProUGUI TextUpgradeName;
    public TextMeshProUGUI TextLv;
    public TextMeshProUGUI TextNeedGold;
    public Button UpgradeButton;

    [HideInInspector] public GoldUpgrade Type;

    public void ChangeType(GoldUpgrade type)
    {
        Type = type;

        string name = "";

        switch (Type)
        {
            case GoldUpgrade.food_grow:
                name = TransMgr.GetText("음식 효율 증가");
                break;
            case GoldUpgrade.grow_time:
                name = TransMgr.GetText("성장 시간 증가");
                break;
            case GoldUpgrade.engery_effect:
                name = TransMgr.GetText("에너지 효율 증가");
                break;

        }

        TextUpgradeName.text = name;
        Refresh();
        UpgradeButton.onClick.AddListener(() =>
        {
            int nowLv = UserDataMgr.Instance.GetGoldUpgrade(Type);
            int needGold = GameStaticValue.NeedGold(nowLv);

            if (UserDataMgr.Instance.IsEnoughGoods(needGold, GoodsType.gold))
            {
                UserDataMgr.Instance.AddGoldUpgrade(Type);
                UserDataMgr.Instance.UseGoods(needGold, GoodsType.gold);
                UI_Lobby.Instance.RefreshTexts();
                DataLoadMgr.SaveLocalData();
                Refresh();
            }
        });
    }

    public void Refresh()
    {
        int nowLv = UserDataMgr.Instance.GetGoldUpgrade(Type);
        int needGold = GameStaticValue.NeedGold(nowLv);

        TextLv.text = string.Format("Lv.{0}", nowLv.ToString());
        string color = UserDataMgr.Instance.IsEnoughGoods(needGold, GoodsType.gold) ? "000000" : "FF0000";
        TextNeedGold.text = string.Format("<color=#{1}>{0}</color>", needGold.ToString(), color);
    }
}
