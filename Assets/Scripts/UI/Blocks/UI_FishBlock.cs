using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FishBlock : MonoBehaviour
{
    public Image ImageBg;
    public Image ImageFish;
    public TextMeshProUGUI TextLv;
    public TextMeshProUGUI TextCount;
    public TextMeshProUGUI TextName;
    public Button ButtonFishDesc;
    public Button ButtonFishUpgrade;
    public TextMeshProUGUI TextUpgrade;
    public GameObject ObjBlock;

    private string fId;

    public void SetFishId(string fid)
    {
        fId = fid;
        Refresh();

        ButtonFishDesc.onClick.AddListener(() =>
        {
            PopupMgr.MakeFishDescPopup(fid);
        });

        ButtonFishUpgrade.onClick.AddListener(() =>
        {
            UpgradeMgr.Instance.AddFishesLv(fid);
        });
    }

    public void Refresh()
    {
        int count = UpgradeMgr.Instance.GetFishesCount(fId);
        int lv = UpgradeMgr.Instance.GetFishesLv(fId);
        int usedCount = 0;

        for (int i = 1; i <= lv; i++)
        {
            usedCount += GameStaticValue.GetNeedFishLvUpCount(i);
        }

        TextCount.text = string.Format("{0}/{1}", (count - usedCount).ToString(), GameStaticValue.GetNeedFishLvUpCount(lv + 1));
        TextLv.text = lv == GameStaticValue.MaxFishLv ? "MAX" : string.Format("Lv.{0}", lv);

        TextName.text = TransMgr.GetText(TableMgr.GetTableString("fish", fId, "t_name"));
        ImageBg.sprite = AtlasMgr.Instance.GetCommonSprite(GameStaticValue.GetGradePath(TableMgr.GetTableInt("fish", fId, "grade")));
        ImageFish.sprite = AtlasMgr.Instance.GetFishesSprite(string.Format("{0}_adult", TableMgr.GetTableString("fish", fId, "res")));

        ObjBlock.SetActive(lv <= 0);

        if (lv <= 0)
        {
            TextUpgrade.text = TransMgr.GetText("획득");
        }
        else
        {
            TextUpgrade.text = TransMgr.GetText("레벨 업");
        }

        ButtonFishUpgrade.interactable = count - usedCount > GameStaticValue.GetNeedFishLvUpCount(lv + 1);
    }
}
