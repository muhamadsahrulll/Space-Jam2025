using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;
    public BannerAds bannerAds;

    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bannerAds.LoadBannerAd();
        //interstitialAds.LoadInterstitialAd();
        //rewardedAds.LoadRewardedAd();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        rewardedAds.LoadRewardedAd();
        interstitialAds.LoadInterstitialAd();

    }

}
