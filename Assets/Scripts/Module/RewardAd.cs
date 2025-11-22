using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;

public class RewardAd
{
    public bool IsLoaded 
    { 
        get
        {
            return rewarded != null && rewarded.CanShowAd();
        }
    }

    public bool IsLoading = false;

    private RewardedAd rewarded;
    private string adUnitId;

    private int Counter = 0;
    private UnityAction SuccessAction;
    private UnityAction FailAction;

    public RewardAd(string adId)
    {
        adUnitId = adId;
        MobileAds.Initialize((initStatue) => 
        {
            LoadRewarded();
        });
    }

    public bool LoadRewarded()
    {
        IsLoading = true;

        if (Counter > 10)
        {
            return false;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        var adRequest = new AdRequest();
        /*
        // 경고: 광고를 로드하지 못했을 때 광고 요청 완료 블록에서 새 광고 로드를 시도하지 마세요.
        // 광고 요청 완료 블록에서 광고를 로드해야 하는 경우에는 네트워크 연결이 제한된 상황 등에서
        // 광고 요청이 계속 실패하지 않도록 광고 로드 재시도 횟수를 제한하세요.
        라고 써져 있었으니, 이것을 고려해서 작성하자.
        */

        RewardedAd.Load(adUnitId, adRequest, (ad, error) =>
        {
            IsLoading = false;

            if (error != null)
            {
                Counter++;
                return;
            }

            Counter = 0;
            rewarded = ad;
        });

        return true;
    }

    public void SetAction(UnityAction successAction, UnityAction failAction)
    {
        SuccessAction = successAction;
        FailAction = failAction;
    }

    public void Show()
    {
        if (IsLoaded)
        {
            rewarded.Show((reward) =>
            {
                LoadRewarded();
                SuccessAction.Invoke();
            });
        }
        else
        {
            Debug.Log("[AdMob] Rewarded not ready");
            FailAction.Invoke();
        }
    }
}
