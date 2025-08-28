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
    public GameObject ObjBlock;

    private string fId;

    public void SetFishId(string fid)
    {
        fId = fid;
        Refresh();
    }

    public void Refresh()
    {
        int count = UpgradeMgr.Instance.GetFishesCount(fId);
        int lv = UpgradeMgr.Instance.GetFishesLv(fId);

        TextCount.text = count.ToString();
        TextLv.text = string.Format("Lv.{0}", lv);

        TextName.text = TableMgr.GetTableString("fish", fId, "name");
        //ImageBg.sprite = TableMgr.GetTableInt("fish", fId, "grade");
        //ImageFish.sprite = TableMgr.GetTableString("fish", fId, "res");

        ObjBlock.SetActive(lv <= 0);
    }
}
