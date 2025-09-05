using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Top : MonoBehaviour
{
    public GameObject ObjGold;
    public Image ImageGold;
    public TextMeshProUGUI TextGold;

    [Space(7)]
    public GameObject ObjFish;
    public Image ImageFishGacha;
    public TextMeshProUGUI TextFishGachaP;

    [Space(7)]
    public GameObject ObjRelic;
    public Image ImageRelicGacha;
    public TextMeshProUGUI TextRelicGachaP;

    public void RefreshText()
    {
        TextGold.text = UserDataMgr.Instance.Gold.ToString();
        TextFishGachaP.text = UserDataMgr.Instance.GachaPoint.ToString();
        TextRelicGachaP.text = UserDataMgr.Instance.RelicPoint.ToString();
    }
}
