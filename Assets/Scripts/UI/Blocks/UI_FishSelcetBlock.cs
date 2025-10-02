using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_FishSelcetBlock : MonoBehaviour
{
    public Button ButtonClick;
    public Image FishImage;
    public TextMeshProUGUI TextSize;
    public TextMeshProUGUI TextSpeed;

    public void SetData(FishData data)
    {
        FishImage.sprite = AtlasMgr.Instance.GetFishesSprite(TableMgr.GetTableString("fish", data.Fid, "res"));
        TextSize.text = string.Format("{0} : {1} <color=#fa0000>+ {2}</color>"
            , TransMgr.GetText("크기"), data.Size, data.GetSizeFromValue(data.AdditionalValue));
        TextSpeed.text = string.Format("{0} : {1} <color=#fa0000>+ {2}</color>"
            , TransMgr.GetText("속도"), data.Speed, data.GetSpeedFromValue(data.AdditionalValue));
    }

    public void SetCallback(UnityAction action)
    {
        ButtonClick.onClick.AddListener(action);
    }
}
