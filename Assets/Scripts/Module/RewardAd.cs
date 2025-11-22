using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardAd
{
    private RewardedAd rewarded;
    private string adUnitId;

    public bool IsLoaded = false;

    public RewardAd(string adId)
    {
        adUnitId = adId;
        LoadRewarded();
    }

    public void LoadRewarded()
    {
        //rewarded = new RewardedAd(adUnitId);

        //rewarded.OnAdLoaded += (sender, args) =>
        //{
        //    Debug.Log("[AdMob] Rewarded loaded");
        //};
        //rewarded.OnAdFailedToLoad += (sender, args) =>
        //{
        //    Debug.LogWarning("[AdMob] Rewarded load failed: " + args.LoadAdError);
        //};
        //rewarded.OnAdFailedToShow += (sender, args) =>
        //{
        //    Debug.LogWarning("[AdMob] Rewarded show failed: " + args.AdError);
        //};
        //rewarded.OnAdClosed += (sender, args) =>
        //{
        //    Debug.Log("[AdMob] Rewarded closed → reload");
        //    LoadRewarded();
        //};
        //rewarded.OnUserEarnedReward += (sender, reward) =>
        //{
        //    Debug.Log($"[AdMob] Rewarded! type={reward.Type}, amount={reward.Amount}");
        //    // 여기에서 보상 지급
        //    // GameManager.Instance.AddGem((int)reward.Amount);
        //};

        //var request = new AdRequest.Builder().Build();
        //rewarded.LoadAd(request);
    }

    public void Show()
    {
        //if (rewarded != null && rewarded.IsLoaded())
        //{
        //    rewarded.Show();
        //}
        //else
        //{
        //    Debug.Log("[AdMob] Rewarded not ready");
        //}
    }
}
