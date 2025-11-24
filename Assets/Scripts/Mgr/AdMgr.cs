using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdMgr : SingletonAllSecen<AdMgr>, IObserver
{
    private RewardAd Rewarded;
    // 구글 예시용 ID
    private string RewardId = "ca-app-pub-3940256099942544/5224354917";
    private AdType Ad = AdType.Reward;

    protected override void SetDataInAwake()
    {
        AdIDs ids = Resources.Load<AdIDs>("AdIds");

        if (ids != null)
        {
#if UNITY_EDITOR
            RewardId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_ANDROID
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
        Ad = AdType.Reward;

        if (Rewarded != null)
        {
            if (Rewarded.IsLoaded)
            { 
                Rewarded.Show();
            }
            else if (Rewarded.IsLoading || Rewarded.LoadRewarded())
            {
                Rewarded.RegistObserver(this);
            }
            else 
            {
                // failaction이 수행될수 있도록하자
                Rewarded.Show();
            }
        }
    }

    public void UpdateObserver()
    {
        // 다시 등록할 수도 있으니까 먼저 해제해주고 
        Rewarded.RemoveObserver(this);

        switch (Ad)
        {
            case AdType.Reward: 
                ShowRewarded(); 
                break;
        }
    }

    private enum AdType
    {
        Reward,
    }
}