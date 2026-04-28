using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    public string androidAdUnitId;

    public static event System.Action OnRewardGranted;

    void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(androidAdUnitId, request, (RewardedAd ad, LoadAdError error) =>
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
                OnRewardGranted?.Invoke();
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
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.Log($"Rewarded Ad Failed to Show: {error.GetMessage()}");
        };
    }

    void OnDestroy()
    {
        rewardedAd?.Destroy();   
    }
}
