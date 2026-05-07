using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    public string androidAdUnitId = "ca-app-pub-8555118095078573/2150622432";

    void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(androidAdUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.Log($"Interstitial Failed to Load: {error.GetMessage()}");
                return;
            }

            interstitialAd = ad;
            SetupCallbacks(ad);
            Debug.Log("Interstitial Loaded");
        });
    }

    public void ShowAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial not ready yet");
            LoadAd();
        }
    }

    private void SetupCallbacks(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstital Opened");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Closed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        };

        ad.OnAdFullScreenContentFailed += (AdError error) => 
        {
            Debug.Log($"Interstitial Failed to Show: {error.GetMessage()}");
        };
    }

    void OnDestroy()
    {
        interstitialAd?.Destroy();
    }
}