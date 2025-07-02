using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
    adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
    adUnitId = androidAdUnitId;
#endif

        Debug.Log("[RewardedAds] Awake - adUnitId = " + adUnitId);
    }

    public void LoadRewardedAd()
    {
        Debug.Log("[RewardedAds] LoadRewardedAd() called, adUnitId = " + adUnitId);

        if (string.IsNullOrEmpty(adUnitId))
        {
            Debug.LogError("[RewardedAds] ERROR - adUnitId is NULL or EMPTY!");
            return;
        }

        Advertisement.Load(adUnitId, this);
    }


    /*public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId, this);
    }*/

    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }

    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ads Initialized...");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

    }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Ads Fully Watched");
        }
    }
    #endregion

}
