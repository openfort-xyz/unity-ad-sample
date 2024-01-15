using System;
using UnityEngine;
using UnityEngine.Events;

public class AdsController : BaseController
{
    public UnityEvent<bool> onAdWatched;

    #region UNITY_LIFECYCLE
    public void Start()
    {
#if UNITY_ANDROID
        string appKey = "";
#elif UNITY_IPHONE
        string appKey = "";
#else
        string appKey = "unexpected_platform";
#endif
        
        Debug.Log("unity-script: IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("unity-script: unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);
    }

    void OnEnable()
    {
        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
        
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }
    
    private void OnDisable()
    {
        //Remove Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;

        //Remove ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent -= ImpressionDataReadyEvent;
        
        //Remove AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
    }
    #endregion

    #region GAME_EVENT_HANDLERS
    public void TokensController_OnTokensGranted_Handler(bool granted)
    {
        viewPanel.SetActive(true);    
    }
    #endregion

    #region PUBLIC_BUTTON_METHODS
    public void WatchAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();   
        }
    }

    public void Skip()
    {
        viewPanel.SetActive(false);
        onAdWatched?.Invoke(false);
    }
    #endregion

    #region AdInfo Rewarded Video
    void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
    }
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdAvailable With AdInfo " + adInfo);
    }
    
    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdUnavailable");
    }

    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent With Error" + ironSourceError + "And AdInfo " + adInfo);
        viewPanel.SetActive(false);
        onAdWatched?.Invoke(false);
    }

    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("unity-script: I got RewardedVideoOnAdRewardedEvent With Placement" + ironSourcePlacement + "And AdInfo " + adInfo);
        viewPanel.SetActive(false);
        onAdWatched?.Invoke(true);
    }
    
    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        // TODO we could do a deeper integration with the ad revenue (usd) regarding to gas fees, etc.
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
    }
    #endregion
}