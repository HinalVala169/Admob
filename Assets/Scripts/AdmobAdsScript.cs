using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using System;

public class AdmobAdsScript : MonoBehaviour
{
    public static AdmobAdsScript Instance { get; private set; }
    public string appId = "ca-app-pub-7509664581678687~3359583826";

#if UNITY_ANDROID
    string bannerId = "ca-app-pub-3940256099942544/6300978111";
    string interId = "ca-app-pub-7509664581678687/5692805959";
    string rewardedId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
    string interId = "ca-app-pub-3940256099942544/4411468910";
    string rewardedId = "ca-app-pub-3940256099942544/1712485313";
#endif

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            // LoadBannerAd();
            // LoadAndShowInterstitialAd();
            // LoadAndShowRewardedAd();
        });
    }

    #region Banner
    public void LoadBannerAd()
    {
        CreateBannerView();
        ListenToBannerEvents();
        if (bannerView == null)
        {
            CreateBannerView();
        }
        var adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    void CreateBannerView()
    {
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
    }

    void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response: " + bannerView.GetResponseInfo());
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error: " + error);
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Banner view paid {adValue.Value} {adValue.CurrencyCode}.");
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
            LogScreenResolution("Banner");
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion

    #region Interstitial
    public void LoadAndShowInterstitialAd()
    {
        // SoundManager.instance.StopBackgroundMusic();
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded!! " + ad.GetResponseInfo());
            interstitialAd = ad;

            InterstitialEvent(interstitialAd);
            ShowInterstitialAd();
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready!!");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}.");
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            LogScreenResolution("Interstitial");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            // SoundManager.instance.PlayBackgroundSound();
            // LoadAndShowInterstitialAd(); // Preload the next ad
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error: " + error);
        };
    }
    #endregion

    #region Rewarded
    public void LoadAndShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded!!");
            rewardedAd = ad;

            RewardedAdEvents(rewardedAd);
            ShowRewardedAd();
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Give reward to player!!");
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready");
        }
    }

    public void RewardedAdEvents(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}.");
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
            LogScreenResolution("Rewarded");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            // LoadAndShowRewardedAd(); // Preload the next ad
            CloseRewardedAdBTN();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error: " + error);
        };
    }

    public void CloseRewardedAdBTN()
    {
        // if(GameManager.instance.timerScript.maxAttamp == 2)
        // {
        //     GameManager.instance.DisableIncrementBTN();
        // }
        // GameManager.instance.timerScript.IncreaseTimer();

        // if(GameManager.instance.timerScript.maxAttamp >= 0 && GameManager.instance.timerScript.maxAttamp < 2)
        // {
        //     GameManager.instance.EnableIncrementBTN();
        // }
        //  SoundManager.instance.PlayBackgroundSound();
    }
    #endregion

    private void LogScreenResolution(string adType)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        Debug.Log($"{adType} ad resolution at the time of ad opening: {screenWidth}x{screenHeight}");
    }
}
