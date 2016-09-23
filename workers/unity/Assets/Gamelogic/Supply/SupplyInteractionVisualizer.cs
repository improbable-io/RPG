using UnityEngine;
using System.Collections;
using Improbable.Supplies;
using Improbable.Unity.Visualizer;

public class SupplyInteractionVisualizer : MonoBehaviour
{

    [Require] private SupplyReader supplyReader;

    public Animation supplyAnimation;

    protected int lastUpdatedCurrentAmount;

    public string supplyHarvestAnimationName = "SupplyHarvestAnimation";
    public string supplyDepletedAnimationName = "SupplyDepletedAnimation";

    void OnEnable()
    {
        supplyReader.CurrentAmountUpdated += OnCurrentAmountUpdated;
    }

    private void OnCurrentAmountUpdated(int amount)
    {
        if (amount <= 0)
        {
            // depleted
            if (lastUpdatedCurrentAmount > amount)
            {
                // This supply **just** got harvested to completion -- animate
                if (supplyAnimation != null)
                {
                    supplyAnimation.Play(supplyDepletedAnimationName);
                }
            }
            else
            {
                // This supply was already harvested to completion before -- fast-forward
                if (supplyAnimation != null && !supplyAnimation.isPlaying)
                {
                    supplyAnimation.Play(supplyDepletedAnimationName);
                    supplyAnimation[supplyDepletedAnimationName].normalizedTime = 1.0f;
                }
            }
        }
        else
        {
            // still harvestable
            if (lastUpdatedCurrentAmount > amount)
            {
                supplyAnimation.Play(supplyHarvestAnimationName);
            }
        }

        lastUpdatedCurrentAmount = amount;
    }

    void OnDisable()
    {
        supplyReader.CurrentAmountUpdated -= OnCurrentAmountUpdated;
    }
}
