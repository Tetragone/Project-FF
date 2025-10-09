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
                name = "food effective";
                break;
            case GoldUpgrade.grow_time:
                name = "grow time up";
                break;
            case GoldUpgrade.fish_move_multi:
                name = "fish move speed up";
                break;

        }

        TextUpgradeName.text = name;
        Refresh();
        UpgradeButton.onClick.AddListener(() =>
        {
            int nowLv = UpgradeMgr.Instance.GetGoldUpgrade(Type);
            int needGold = GameStaticValue.NeedGold(nowLv);

            if (UserDataMgr.Instance.IsEnoughGoods(needGold, GoodsType.gold))
            {
                UpgradeMgr.Instance.AddGoldUpgrade(Type);
                UserDataMgr.Instance.UseGoods(needGold, GoodsType.gold);
                UI_Lobby.Instance.RefreshTexts();
                DataLoadMgr.SaveLocalData();
                Refresh();
            }
        });
    }

    public void Refresh()
    {
        int nowLv = UpgradeMgr.Instance.GetGoldUpgrade(Type);
        int needGold = GameStaticValue.NeedGold(nowLv);

        TextLv.text = string.Format("Lv.{0}", nowLv.ToString());
        TextNeedGold.text = needGold.ToString();
    }
}
