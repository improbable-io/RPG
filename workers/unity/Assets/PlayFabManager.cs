using UnityEngine;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Events;

using PlayFabResult = PlayFab.ClientModels.LoginResult;

public delegate void OnLoginHandler(PlayFabResult result);

public class PlayFabManager : MonoBehaviour
{
    static PlayFabManager mInstance = null;

    // This accessor will return a PlayFabManager if the following conditions are true:
    //  1) A PlayFab game object exists (which should be in the ClientScene but not the PhysicsScene)
    //  2) The PlayFabManager script on the PlayFab game object is enabled
    //
    // If any of these conditions do not hold, then this accessor will return a null pointer
    public static PlayFabManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject playfab = GameObject.Find("PlayFab");
                if (playfab != null)
                {
                    PlayFabManager component = playfab.GetComponent<PlayFabManager>();
                    if (component.enabled)
                        mInstance = component;
                }
            }
            return mInstance;
        }
    }

    public event OnLoginHandler OnLoginEvent;
    public string playFabId;

    public void OnLogin(LoginResult result)
    {
        if (OnLoginEvent != null)
        {
            OnLoginEvent(result);
            this.playFabId = result.PlayFabId;
        }
    }

    public void Login(bool createAccount = true)
    {
        // Connect to PlayFab
        PlayFabEvents playFabEvents = PlayFabEvents.Init();
        playFabEvents.OnLoginResultEvent += OnLogin;
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest() { CreateAccount = createAccount, CustomId = SystemInfo.deviceUniqueIdentifier },
            null,
            null
        );
    }
}
