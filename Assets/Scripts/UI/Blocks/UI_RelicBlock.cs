using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RelicBlock : MonoBehaviour
{
    public Image ImageBg;
    public Image ImageRelic;
    public TextMeshProUGUI TextLv;
    public TextMeshProUGUI TextCount;
    public TextMeshProUGUI TextName;
    public Button ButtonRelicDesc;
    public Button ButtonRelicUpgrade;
    public GameObject ObjBlock;

    private string rId;

    public void SetRelicId(string rid)
    {
        rId = rid;
        Refresh();

        ButtonRelicDesc.onClick.AddListener(() =>
        {

        });

        ButtonRelicUpgrade.onClick.AddListener(() =>
        {
            UpgradeMgr.Instance.AddRelicLv(rid);
        });
    }

    public void Refresh()
    {
        int count = UpgradeMgr.Instance.GetRelicCount(rId);
        int lv = UpgradeMgr.Instance.GetRelicLv(rId);
        int usedCount = 0;

        for (int i = 1; i <= lv; i++)
        {
            usedCount += GameStaticValue.RelicLvUpCount(i);
        }
        TextCount.text = string.Format("{0}/{1}", (count - usedCount).ToString(), GameStaticValue.RelicLvUpCount(lv + 1));
        TextLv.text = lv == GameStaticValue.MaxRelicLv ? "MAX" : string.Format("Lv.{0}", lv);

        TextName.text = TableMgr.GetTableString("relic", rId, "name");
        ImageBg.sprite = AtlasMgr.Instance.GetCommonSprite(GameStaticValue.GetGradePath(TableMgr.GetTableInt("relic", rId, "grade")));
        ImageRelic.sprite = AtlasMgr.Instance.GetFishesSprite(TableMgr.GetTableString("relic", rId, "res"));

        ObjBlock.SetActive(lv <= 0);
    }
}
