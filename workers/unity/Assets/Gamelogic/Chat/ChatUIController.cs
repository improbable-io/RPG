using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Improbable.Checks;
using Improbable.Unity.Visualizer;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatUIController : MonoBehaviour
{

    [Require]
    private CheckIsClientSideWriter isClientSideWriter;

    public Text[] chatBroadcastBuffer;
    public InputField inputText;

    public System.Action<string> OnChatMessageSubmitcCallback;

    void OnEnable()
    {
        this.inputText = this.GetComponentInChildren<InputField>();
    }

    public void OnChatMessageSubmit()
    {
        Debug.Log("[ChatUIController] OnChatMessageSubmit()");

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {

            if (this.inputText.text.Length > 0)
            {
                if (OnChatMessageSubmitcCallback != null)
                {
                    Debug.Log("Submit");
                    OnChatMessageSubmitcCallback(this.inputText.text);
                }
            }
          
            inputText.text = "";
            inputText.ActivateInputField();
        }
        else
        {
            Debug.Log("Cancelled");
        }
        
    }

  

    public bool IsChatPanelVisible()
    {
        return gameObject.activeSelf;
    }

    public bool IsTypingFocus()
    {
        return IsChatPanelVisible() && this.inputText.isFocused;
    }

    public void updateBroadcasts(List<string> messageHistory)
    {
        int bufferCount = 0;
        for (int messageIndex = messageHistory.Count - 1; messageIndex >= 0 && bufferCount < chatBroadcastBuffer.Length; messageIndex--)
        {
            if (bufferCount < messageHistory.Count)
            {
                chatBroadcastBuffer[chatBroadcastBuffer.Length - 1 - bufferCount++].text = messageHistory[messageIndex];
            }
            
        }
        for (; bufferCount < chatBroadcastBuffer.Length; bufferCount++)
        {
            chatBroadcastBuffer[chatBroadcastBuffer.Length-1-bufferCount].text = "";
        }
    }
}
