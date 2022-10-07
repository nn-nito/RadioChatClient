using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdvertisementManager : MonoBehaviour, IUnityAdsListener
{
    private const string AppStore = "*****";
    private const string GooglePlay = "*******";
    private string myPlacementId = "rewardedVideo";
    public bool enableTestMode = true;

    IEnumerator Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.AddListener(this);
            var gameId = GooglePlay;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                gameId = AppStore;
            }
            if (Application.platform == RuntimePlatform.Android)
            {
                gameId = GooglePlay;
            }
            Advertisement.Initialize(gameId, enableTestMode);
        }

        // Wait until Unity Ads is initialized,
        //  and the default ad placement is ready.
        while (Advertisement.isInitialized == false || Advertisement.IsReady(myPlacementId) == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Show the default ad placement.
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            SceneManager.LoadScene(SceneList.HOME);
            // Reward the user for watching the ad to completion.
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            SceneManager.LoadScene(SceneList.HOME);
        }
        else if (showResult == ShowResult.Failed)
        {
            SceneManager.LoadScene(SceneList.HOME);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}