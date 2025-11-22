using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMgr : SingletonAllSecen<AdMgr>
{
    private RewardAd Rewarded;
    private string RewardId = "";

    protected override void SetDataInAwake()
    {
        AdIDs ids = Resources.Load<AdIDs>("AdIds");

#if UNITY_ANDROID
        RewardId = ids.AndoridRewardId;
#elif UNITY_IOS
        RewardId = ids.IOSRewardId;
#endif

        Rewarded = new RewardAd(RewardId);
    }

    public void ShowRewarded()
    {
        if (Rewarded != null && Rewarded.IsLoaded)
        {
            // 호출이 안된 경우도 필요함
            Rewarded.Show();
        }
    }
}