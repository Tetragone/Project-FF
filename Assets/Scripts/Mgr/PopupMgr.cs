using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class PopupMgr
{
    // TODO : make wait popup
    public static async void MakeCommonPopup(string title, string content, bool isUseNo, bool IsUseClose, UnityAction action)
    {
        GameObject res = await AddressableMgr.LoadAndInstantiate("popup_common", UI_Lobby.Root.transform, false);
        PopupCommon popup = res.GetComponent<PopupCommon>();

        popup.SetReleaseObj(res);
        popup.SetText(title, content);
        popup.AddYesListener(action);
        popup.InitAfterSetting(true, isUseNo, IsUseClose);
    }

    public static async void MakeFishDescPopup(string fid)
    {
        GameObject res = await AddressableMgr.LoadAndInstantiate("fish_help", UI_Lobby.Root.transform, false);
        PopupCommon popup = res.GetComponent<PopupCommon>();

        popup.SetReleaseObj(res);
        string title = TransMgr.GetText(TableMgr.GetTableString("fish", fid, "name"));
        string content = TransMgr.GetText(TableMgr.GetTableString("fish", fid, "desc"));
        popup.SetText(title, content);
        popup.InitAfterSetting(true, false, true);
        popup.CustomObj[0].GetComponent<Image>().sprite = AtlasMgr.Instance.GetFishesSprite(TableMgr.GetTableString("fish", fid, "res"));
    }

    public static async void MakeFishSelectPopup(Dictionary<string, FishData> data)
    {
        GameObject res = await AddressableMgr.LoadAndInstantiate("fish_select", UI_Lobby.Root.transform, false);
        PopupCommon popup = res.GetComponent<PopupCommon>();
        popup.SetReleaseObj(res);

        string content = TransMgr.GetText("레이스에 나갈 물고기를 선택해주세요.");
        popup.SetText("", content);
        popup.InitAfterSetting(false, false, true);

        GameObject resIcon = popup.CustomObj[0];
        foreach (string key in data.Keys)
        {
            GameObject block = GameObject.Instantiate(resIcon, popup.CustomObj[1].transform);
            block.GetComponent<UI_FishSelcetBlock>().SetData(data[key]);
            block.GetComponent<UI_FishSelcetBlock>().SetCallback(() =>
            {
                AquaMgr.Instance.SelectFish(data[key]);
            });
            block.SetActive(true);
        }
    }
}
