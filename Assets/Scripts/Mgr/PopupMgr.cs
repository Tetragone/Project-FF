using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PopupMgr
{
    public static void MakeCommonPopup(string title, string content, bool isUseNo, bool IsUseClose, UnityAction action)
    {
        // TODO : need change to addressable
        PopupCommon res = Resources.Load<PopupCommon>("Prefabs/UI/popup_common");
        PopupCommon popup = Object.Instantiate<PopupCommon>(res, UI_Lobby.Root.transform);

        popup.SetText(title, content);
        popup.AddYesListener(action);
        popup.InitAfterSetting(isUseNo, IsUseClose);
    }

    public static void MakeFishDescPopup(string fid)
    {
        // TODO : need change to addressable
        PopupCommon res = Resources.Load<PopupCommon>("Prefabs/UI/fish_help");
        PopupCommon popup = Object.Instantiate<PopupCommon>(res, UI_Lobby.Root.transform);

        string title = TableMgr.GetTableString("fish", fid, "name");
        string content = TableMgr.GetTableString("fish", fid, "desc");
        popup.SetText(title, content);
        popup.InitAfterSetting(false, true);
    }
}
