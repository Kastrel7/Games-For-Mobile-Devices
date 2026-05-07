using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections.Generic;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    private string _adUnitId = "ca-app-pub-8555118095078573/8113395550";

    public static event System.Action OnRewardGranted;

    private static readonly Queue<System.Action> _mainThreadQueue = new Queue<System.Action>();

    void Start()
    {
        LoadAd();
    }

    void Update()
    {
        // Execute any queued actions on the main thread
        while (_mainThreadQueue.Count > 0)
        {
            _mainThreadQueue.Dequeue().Invoke();
        }
    }

    private void RunOnMainThread(System.Action action)
    {
        _mainThreadQueue.Enqueue(action);
    }

    public void LoadAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.Log($"Rewarded Ad Failed to Load: {error.GetMessage()}");
                return;
            }

            rewardedAd = ad;
            SetupCallbacks(ad);
            Debug.Log("Rewarded Ad Loaded");
        });
    }

    public void ShowAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Reward Granted!");
                RunOnMainThread(() =>
                {
                    OnRewardGranted?.Invoke();
                });
            });
        }
        else
        {
            Debug.Log("Rewarded Ad not ready yet");
            LoadAd();
        }
    }

    private void SetupCallbacks(RewardedAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded Ad Opened");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad Closed");
            RunOnMainThread(() =>
            {
                LoadAd();
            });
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.Log($"Rewarded Ad Failed to Show: {error.GetMessage()}");
            RunOnMainThread(() =>
            {
                LoadAd();
            });
        };
    }

    void OnDestroy()
    {
        rewardedAd?.Destroy();
    }
}