using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdManager : MonoBehaviour
{
    public string androidAdUnitId = "ca-app-pub-8555118095078573/6915863950";
    private BannerView bannerView;

    void Start()
    {
        LoadBanner();
    }

    public void LoadBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(androidAdUnitId, AdSize.Banner, AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += OnBannerLoaded;
        bannerView.OnBannerAdLoadFailed += OnBannerError;
        bannerView.OnAdClicked += OnBannerClicked;

        bannerView.LoadAd(new AdRequest());
    }

    public void HideBannerAd()
    { 
        bannerView?.Hide(); 
    }

    public void ShowBannerAd() 
    {
        bannerView?.Show(); 
    }

    void OnBannerLoaded()
    { 
        Debug.Log("Banner Loaded"); 
    }

    void OnBannerError(LoadAdError error)
    { 
        Debug.Log($"Banner Failed to Load: {error.GetMessage()}"); 
    }

    void OnBannerClicked()
    { 
        Debug.Log("Banner Clicked"); 
    }

    private void OnDestroy()
    {
        bannerView?.Destroy();
    }
}
