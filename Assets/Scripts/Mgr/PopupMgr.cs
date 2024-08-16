using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PopupMgr
{
    public static void MakePopupEndGame(int gold)
    {
        PopupCommon res = Resources.Load<PopupCommon>("Prefabs/UI/popup_common");
        PopupCommon popup = Object.Instantiate<PopupCommon>(res);

        popup.SetText("", gold.ToString());
        popup.AddYesListener(() =>
        {
            UserDataMgr.Instance.AddGoods(gold, "gold");
            RaceMgr.Instance.EndGame();
        });
        popup.InitAfterSetting(false, false);
    }
}
