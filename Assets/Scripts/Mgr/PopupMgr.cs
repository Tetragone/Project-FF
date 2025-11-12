using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class PopupMgr
{
    public static void ActiveLoadingPopup(bool isActive)
    {
        UI_Lobby.Instance.Loading.SetActive(isActive);
    }

    public static async void MakeCommonPopup(string title, string content, bool isUseNo, bool IsUseClose, UnityAction action)
    {
        ActiveLoadingPopup(true);
        GameObject res = await AddressableMgr.LoadAndInstantiate("popup_common", UI_Lobby.Root.transform, false);
        ActiveLoadingPopup(false);
        PopupCommon popup = res.GetComponent<PopupCommon>();

        popup.SetReleaseObj(res);
        popup.SetText(title, content);
        popup.AddYesListener(action);
        popup.InitAfterSetting(true, isUseNo, IsUseClose);
    }

    public static async void MakeFishDescPopup(string fid)
    {
        ActiveLoadingPopup(true);
        GameObject res = await AddressableMgr.LoadAndInstantiate("fish_help", UI_Lobby.Root.transform, false);
        ActiveLoadingPopup(false);
        PopupCommon popup = res.GetComponent<PopupCommon>();

        popup.SetReleaseObj(res);
        string title = TransMgr.GetText(TableMgr.GetTableString("fish", fid, "t_name"));
        string content = TransMgr.GetText(TableMgr.GetTableString("fish", fid, "t_desc"));
        popup.SetText(title, content);
        popup.InitAfterSetting(true, false, true);
        popup.CustomObj[0].GetComponent<Image>().sprite = AtlasMgr.Instance.GetFishesSprite(TableMgr.GetTableString("fish", fid, "res"));
    }

    public static async void MakeFishSelectPopup(Dictionary<string, FishData> data, UnityAction action)
    {
        ActiveLoadingPopup(true);
        GameObject res = await AddressableMgr.LoadAndInstantiate("fish_select", UI_Lobby.Root.transform, false);
        ActiveLoadingPopup(false);
        PopupCommon popup = res.GetComponent<PopupCommon>();
        popup.SetReleaseObj(res);

        string content = TransMgr.GetText("레이스에 나갈 물고기를 선택해주세요.");
        popup.SetText("", content);
        popup.AddYesListener(action);
        popup.InitAfterSetting(true, false, true);

        popup.SetButtonText(TransMgr.GetText("선택"));
        popup.ButtonYes.interactable = false;
        GameObject resIcon = popup.CustomObj[0];
        foreach (string key in data.Keys)
        {
            GameObject block = GameObject.Instantiate(resIcon, popup.CustomObj[1].transform);
            block.GetComponent<UI_FishSelcetBlock>().SetData(data[key]);
            block.GetComponent<UI_FishSelcetBlock>().SetCallback(() =>
            {
                AquaMgr.Instance.SelectFish(data[key]);
                popup.ButtonYes.interactable = true;
            });
            block.SetActive(true);
        }
    }

    public static async void MakeOptionPopup()
    {
        ActiveLoadingPopup(true);
        GameObject res = await AddressableMgr.LoadAndInstantiate("popup_setting", UI_Lobby.Root.transform, false);
        ActiveLoadingPopup(false);
        PopupCommon popup = res.GetComponent<PopupCommon>();
        popup.SetReleaseObj(res);

        string title = TransMgr.GetText("옵션");
        popup.SetText(title, "");
        popup.InitAfterSetting(false, false, true);
    }

    public static async void MakeLoginPopup()
    {
        ActiveLoadingPopup(true);
        GameObject res = await AddressableMgr.LoadAndInstantiate("popup_login", UI_Lobby.Root.transform, false);
        ActiveLoadingPopup(false);
        PopupCommon popup = res.GetComponent<PopupCommon>();
        popup.SetReleaseObj(res);

        string title = TransMgr.GetText("이메일 로그인");
        popup.SetText(title, "");
        popup.InitAfterSetting(false, false, true);
        FireAuth.Instance.RegistObserver(popup);
        popup.AddObserverAction(() =>
        {
            popup.ButtonClose.onClick.Invoke();
            FireAuth.Instance.RemoveObserver(popup);
        }, true);
    }
}
