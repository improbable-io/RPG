using System;
using UnityEngine;
using Improbable.Collections;
using Improbable.Checks;
using Improbable.Player;
using Improbable.Supplies;
using Improbable.Unity.Visualizer;

[Serializable]
public struct InventorySupplyQuantityEntry
{
    public string supplyType;
    public int quantity;
}

public class PlayerInventoryVisualizer : MonoBehaviour
{
    public List<InventorySupplyQuantityEntry> inventorySupplyQuantityEntries = new List<InventorySupplyQuantityEntry>();

    [Require]
    private PlayerInventoryReader playerInventory;

    [Require]
    private CheckIsClientSideWriter checkIsClientSideWriter;

    void OnEnable()
    {
        playerInventory.SupplyQuantityMapUpdated += OnSupplyQuantityMapUpdated;
    }

    private void OnSupplyQuantityMapUpdated(Map<SupplyTypeEnum, int> supplyQuantityMap)
    {
        inventorySupplyQuantityEntries.Clear();

        foreach (var supplyTypeQuantityPair in supplyQuantityMap)
        {
            InventorySupplyQuantityEntry newEntry = new InventorySupplyQuantityEntry();
            newEntry.supplyType = supplyTypeQuantityPair.Key.ToString();
            newEntry.quantity = supplyTypeQuantityPair.Value;
            inventorySupplyQuantityEntries.Add(newEntry);
        }
    }

    void OnDisable()
    {
        playerInventory.SupplyQuantityMapUpdated += OnSupplyQuantityMapUpdated;
    }

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(10, Screen.height/2, 128, 192), "Player Inventory");

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleLeft;
        style.normal.textColor = Color.white;

        int i = 0;
        foreach (var inventorySupplyQuantityEntry in inventorySupplyQuantityEntries)
        {
            i++;
            GUI.Label(new Rect(24, Screen.height/2 + 24 + 16*i, 128, 24), inventorySupplyQuantityEntry.supplyType + ": " + inventorySupplyQuantityEntry.quantity, style);
        }
    }
}
