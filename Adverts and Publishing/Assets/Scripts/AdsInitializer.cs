using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    public bool testMode = true;

    void Awake()
    {
        InitializeUnityAds();
        InitializeAdMob();
    }

    void InitializeUnityAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(androidGameId, testMode, this);
        }
    }

    void InitializeAdMob()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialized");
        });
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
