using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Actions;
using Improbable.Checks;
using Improbable.Supplies;
using Improbable.Unity.Visualizer;

public class PlayerUIController : MonoBehaviour
{
    [Require]
    CheckIsClientSideWriter clientSideCheck;  // Ensures that this runs on the client only

    // References to UI prefabs in the scene canvas
    public GameObject interactionUI;
    public GameObject shopUI;

    void Start()
    {
        interactionUI = GameObject.Find("HUDCanvas").transform.GetChild(0).gameObject;
        interactionUI.GetComponent<InteractionOptionsUIBehaviour>().playerController = this.GetComponent<PlayerController>();

        shopUI = GameObject.Find("HUDCanvas").transform.Find("ShopUI_Singleton").gameObject;
        shopUI.GetComponent<ShopUIBehaviour>().playerController = this.GetComponent<PlayerController>();
    }

    public void showInteractionUI(GameObject interactionObject, List<ActionTypeEnumWrapper> availableActions)
    {
        hideAllUI();
        interactionUI.GetComponent<InteractionOptionsUIBehaviour>()
            .showInteractionOptions(interactionObject, availableActions);
    }

    public void hideInteractionUI()
    {
        interactionUI.GetComponent<InteractionOptionsUIBehaviour>().hideInteractionOptions();
    }

    public void showShopUI(List<SupplyTypeEnum> productList)
    {
        hideAllUI();
        shopUI.GetComponent<ShopUIBehaviour>()
            .showShopUI(productList);
    }

    public void hideShopUI()
    {
        shopUI.GetComponent<ShopUIBehaviour>().hideShopUI();
    }

    public void hideAllUI()
    {
        hideShopUI();
        hideInteractionUI();
    }
}
