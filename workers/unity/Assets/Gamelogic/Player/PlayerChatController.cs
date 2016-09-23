using System;
using UnityEngine;
using Improbable.Chat;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Improbable.Collections;
using Improbable.Unity;

[EngineType(EnginePlatform.Client)]

public class PlayerChatController : MonoBehaviour {

	[Require]
    private ChatBroadcastRequestWriter chatBroadcastRequestWriter;

    [Require]
    private ChatBroadcastListenerReader chatBroadcastListener;

    public List<String> messageHistory;

    public ChatUIController chatUIController;

    void Awake()
    {
        chatUIController = GameObject.Find("HUDCanvas").GetComponentInChildren<ChatUIController>();
    }

    void OnEnable()
    {
        chatBroadcastListener.MessageHistoryUpdated += onMessageHistoryUpdated;
        chatUIController.OnChatMessageSubmitcCallback += OnChatMessageSubmitCallback;
    }

    void OnDisable()
    {
        chatBroadcastListener.MessageHistoryUpdated -= onMessageHistoryUpdated;
        chatUIController.OnChatMessageSubmitcCallback -= OnChatMessageSubmitCallback;
    }

    private void OnChatMessageSubmitCallback(string broadcastMessageContent)
    {
        TriggerChatBroadcast(broadcastMessageContent);
    }

    private void onMessageHistoryUpdated(List<string> readOnlyList)
    {
        messageHistory = new List<string>(readOnlyList);
        chatUIController.updateBroadcasts(messageHistory);
    }

    public void TriggerChatBroadcast(String messageContent)
    {
        chatBroadcastRequestWriter.Update.TriggerRequestChatBroadcast(UnityWorker.Configuration.EngineId + ": " + messageContent).FinishAndSend();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && !chatUIController.IsTypingFocus())
        {
            ToggleChatPanel(true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!chatUIController.IsTypingFocus())
            {
                chatUIController.inputText.ActivateInputField();
            }
        }
    }

    public void ToggleChatPanel(bool giveFocus = false)
    {
        chatUIController.gameObject.SetActive(!chatUIController.IsChatPanelVisible());
        if (chatUIController.IsChatPanelVisible())
        {
            if (giveFocus)
            {
                chatUIController.inputText.ActivateInputField();
            }
        }
        else
        {
            chatUIController.inputText.DeactivateInputField();
        }
    }

}
