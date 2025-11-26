using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

public class RewardAd: ISubject
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
    private List<IObserver> Observers = new List<IObserver>();

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

        RewardedAd.Load(adUnitId, adRequest, (ad, error) =>
        {
            IsLoading = false;
            UpdateObserver();
            
            if (error != null)
            {
                Counter++;
                return;
            }

            InitReloadCounter();
            rewarded = ad;
        });

        return true;
    }

    public void InitReloadCounter()
    {
        Counter = 0;
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

    public void UpdateObserver()
    {
        foreach (var observer in Observers)
        {
            observer.UpdateObserver();
        }
    }

    public void RegistObserver(IObserver obverser)
    {
        if (!Observers.Contains(obverser))
        {
            Observers.Add(obverser);
        }
    }

    public void RemoveObserver(IObserver obverser)
    {
        if (Observers.Contains(obverser))
        {
            Observers.Remove(obverser);
        }
    }
}
