using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdMgr : SingletonAllSecen<AdMgr>
{
    private RewardAd Rewarded;
    private string RewardId = "ca-app-pub-3940256099942544/5224354917";

    protected override void SetDataInAwake()
    {
        AdIDs ids = Resources.Load<AdIDs>("AdIds");

        if (ids != null)
        {
#if UNITY_ANDROID
            RewardId = ids.AndoridRewardId;
#elif UNITY_IOS
            RewardId = ids.IOSRewardId;
#endif
        }

        Rewarded = new RewardAd(RewardId);
    }

    public void SetRewardAction(UnityAction successAction, UnityAction failAction)
    {
        Rewarded.SetAction(successAction, failAction);
    }

    public void ShowRewarded()
    {
        if (Rewarded != null && Rewarded.IsLoaded)
        {
            // 호출이 안된 경우도 필요함
            Rewarded.Show();
        }
    }


    // reloading 기달리는것을 구현해야함
    private void Update()
    {
    }
}