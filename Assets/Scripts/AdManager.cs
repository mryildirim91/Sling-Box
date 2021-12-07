using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
{
    private readonly string _androidGameId = "4493163";
    private readonly string _iOsGameId = "4493162";
    private bool _testMode = false;
    private string _gameId;

    private readonly string _androidAdUnitId = "Interstitial_Android";
    private readonly string _iOsAdUnitId = "Interstitial_iOS";
    private string _adUnitId;

    private void Awake()
    {
        InitializeAds();
    }

    private void OnEnable()
    {
        EventManager.OnLevelComplete += LoadAd;
    }

    private void OnDisable()
    {
        EventManager.OnLevelComplete -= LoadAd;
    }

    private void InitializeAds()
    {
        _gameId = Application.platform == RuntimePlatform.IPhonePlayer ? _iOsGameId : _androidGameId;
        
        Advertisement.Initialize(_gameId, _testMode, this);
    }
    
    private void LoadAd()
    {
        var level = PlayerPrefs.GetInt("Level");
        
        if(level < 4 && level % 2 != 0) return;
        
        StartCoroutine(AdDelay());
    }

    private IEnumerator AdDelay()
    {
        if (!Advertisement.isInitialized) yield break;
        
        yield return new WaitForSeconds(1);
        
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
        
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
        
        ShowAd();
    }
    
    private void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Load Fail");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Show Failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Show Start");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Show Click");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Show Complete");
    }
}
