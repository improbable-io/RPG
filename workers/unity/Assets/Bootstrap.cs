using System;
using Improbable.Unity;
using Improbable.Unity.Configuration;
using Improbable.Unity.Core;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    /// <summary>
    /// Allow users to edit the configuration settings via the Unity's Inspector.
    /// </summary>
    public WorkerConfigurationData Configuration = new WorkerConfigurationData();

    public void Start()
    {
        // Client should wait for callback from login splashscreen, FSim should start immediately
        if (Configuration.SpatialOsApplication.EnginePlatform == EnginePlatform.FSim)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        // Apply the configuration settings.
        UnityWorker.ApplyConfiguration(Configuration);

        // UnityWorker.TemplateProvider = <custom template provider>;
        // UnityWorker.AssetsToPrePool = <list of assets to prepool>;
        // UnityWorker.AssetsToPrecache = <list of assets to precache>;

        switch (UnityWorker.Configuration.EnginePlatform)
        {
            case EnginePlatform.FSim:
                // The UnitySDK no longer automatically exits upon disconnection.
                UnityWorker.OnDisconnected += reason => Application.Quit();

                // The UnitySDK no longer manages the framerate automatically.
                var targetFramerate = 120;
                var fixedFramerate = 20;

                // Please see https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html for more information.
                Application.targetFrameRate = targetFramerate;

                // Please see https://docs.unity3d.com/ScriptReference/Time-fixedDeltaTime.html for more information.
                Time.fixedDeltaTime = 1.0f/fixedFramerate;
                break;

            case EnginePlatform.Client:
                // UnityWorker.OnDisconnected += reason => <Return to main menu, etc.>;
                break;
        }

        // Start connecting to SpatialOS.
        UnityWorker.Connect(gameObject);
    }

    public void OnClientConnectButton()
    {
        if (PlayFabManager.Instance)
        {
            GameObject userIdField = GameObject.Find("HUDCanvas/SplashScreen/UserIdInput/Text");
            String thisUserId = userIdField.GetComponent<Text>().text;
            Configuration.Networking.MetaData.Add("userId", thisUserId);
            // Callbacks after PlayFab login
            PlayFabManager.Instance.OnLoginEvent += r => addPlayFabMetadata(r.PlayFabId);
            PlayFabManager.Instance.OnLoginEvent += r => StartGame();

            PlayFabManager.Instance.Login();
        }
        else
        {
            Debug.Log("PLAYFAB IS MISSING OR DISABLED; CANNOT START GAME");
        }
    }

    public void addPlayFabMetadata(string playFabId)
    {
        Configuration.Networking.MetaData.Add("playFabId", playFabId);
    }
}