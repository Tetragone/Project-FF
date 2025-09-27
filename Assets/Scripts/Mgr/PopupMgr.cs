using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
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
        popup.InitAfterSetting(isUseNo, IsUseClose);
    }

    public static async void MakeFishDescPopup(string fid)
    {
        GameObject res = await AddressableMgr.LoadAndInstantiate("fish_help", UI_Lobby.Root.transform, false);
        PopupCommon popup = res.GetComponent<PopupCommon>();

        popup.SetReleaseObj(res);
        string title = TableMgr.GetTableString("fish", fid, "name");
        string content = TableMgr.GetTableString("fish", fid, "desc");
        popup.SetText(title, content);
        popup.InitAfterSetting(false, true);
        popup.CustomObj[0].GetComponent<Image>().sprite = AtlasMgr.Instance.GetFishesSprite(TableMgr.GetTableString("fish", fid, "res"));
    }
}
