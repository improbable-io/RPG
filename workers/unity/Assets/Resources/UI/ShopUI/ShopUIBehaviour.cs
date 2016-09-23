using UnityEngine;
using System.Collections.Generic;
using Improbable.Checks;
using Improbable.Supplies;
using Improbable.Unity.Visualizer;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUIBehaviour : UIBehaviour
{
    [Require]
    CheckIsClientSideWriter clientSideCheck; // Ensures that this runs on the client only

    // Set during initialisation of PlayerUIController
    public PlayerController playerController;

    public GameObject shopProductRowPrefab;

    private List<GameObject> shopProductRows = new List<GameObject>();
    public Canvas hudCanvas;

    private GameObject scrollViewContent;

    protected override void Start()
    {
        hideShopUI();
        // Setup Shop Close Button
        GameObject shopCloseButton = this.transform.Find("CloseShopUIButton").gameObject;
        shopCloseButton.GetComponent<Button>().onClick.AddListener(hideShopUI);
        // Cache reference to scroll view's content game object
        scrollViewContent = this.transform.Find("ProductScrollView/Viewport/Content").gameObject;
    }

    public void showShopUI(List<SupplyTypeEnum> availableProducts)
    {
        // (Re-)Populate scrollview with a child rows for each available product
        destroyProductRows();
        for (int i = 0; i < availableProducts.Count; i++)
        {
            int local_i = i;
            GameObject nextProductRow = Instantiate(shopProductRowPrefab);
            nextProductRow.gameObject.SetActive(true);
            shopProductRows.Add(nextProductRow);
            // Scroll-view's vertical layout group will specify exact positioning
            nextProductRow.transform.SetParent(scrollViewContent.transform, false);
            // Product Name
            GameObject productNameElement = nextProductRow.transform.Find("ProductNameText").gameObject;
            Text rowText = productNameElement.GetComponentInChildren<Text>();
            rowText.text = availableProducts[local_i].ToString();
            // Set Up Buy Button
            GameObject buyButtonElement = nextProductRow.transform.Find("BuyButton").gameObject;
            buyButtonElement.GetComponent<Button>().onClick.AddListener(() =>
            {
                // Coins to Supply Type
                playerController.TriggerTransactionRequest(SupplyTypeEnum.COINS, availableProducts[local_i]);
            });
            // Set Up Sell Button
            GameObject sellButtonElement = nextProductRow.transform.Find("SellButton").gameObject;
            sellButtonElement.GetComponent<Button>().onClick.AddListener(() =>
            {
                // Supply Type to Coins
                playerController.TriggerTransactionRequest(availableProducts[local_i], SupplyTypeEnum.COINS);
            });
        }
        // Show
        transform.gameObject.SetActive(true);
    }

    public void hideShopUI()
    {
        this.transform.gameObject.SetActive(false);
        destroyProductRows();
    }

    // TODO: Pool rows instead of re-instantiating
    private void destroyProductRows()
    {
        while (shopProductRows.Count > 0)
        {
            GameObject forDestruction = shopProductRows[0];
            shopProductRows.RemoveAt(0);
            Destroy(forDestruction);
        }
    }
}
