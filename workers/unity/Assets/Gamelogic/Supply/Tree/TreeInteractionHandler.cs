using UnityEngine;
using System.Collections.Generic;
using Improbable.Unity.Visualizer;
using Improbable.Actions;
using Improbable.Supplies;

public class TreeInteractionHandler : InteractionHandler
{
    [Require]
    ActionAvailabilityReader actionAvailabilityReader;

    List<ActionTypeEnumWrapper> availableActions;

    [Require]
    private SupplyReader supplyReader;

    void OnEnable()
    {
        actionAvailabilityReader.AvailableActionsListUpdated += OnAvailableActionsListUpdated;
    }

    void OnDisable()
    {
        actionAvailabilityReader.AvailableActionsListUpdated -= OnAvailableActionsListUpdated;
    }

    private void OnAvailableActionsListUpdated(List<ActionTypeEnumWrapper> possibleActions)
    {
        this.availableActions = new List<ActionTypeEnumWrapper>(possibleActions);
    }

    public override bool isInteractable()
    {
        return this.availableActions.Count > 0 && !supplyReader.IsDepletable || (supplyReader.IsDepletable && supplyReader.CurrentAmount > 0);
    }

    public int getNumberOfAvailableActions()
    {
        return availableActions.Count;
    }

    public override void performInteraction(PlayerController playerController, ActionTypeEnum interaction)
    {
        Debug.Log("performInteraction");
        playerController.playerInteractionController.interactContinuously(this, interaction, supplyReader.HarvestDuration);
    }

    public override void onContinuousInteractionStep(PlayerController playerController, ActionTypeEnum interaction)
    {
        Debug.Log("onContinuousInteractionStep");
        playerController.TriggerActionRequest(this.gameObject.EntityId(), interaction);
        playerController.TriggerActionAnimation(interaction);
    }

    public override void onPlayerBeginInteraction(PlayerController playerController)
    {
        if (availableActions.Count == 0)
        {
            Debug.LogWarning("No available actions");
        }
        else if (availableActions.Count == 1)
        {
            Debug.Log("Possible actions count is 1; just going to harvest");
            playerController.MoveToAndInteract(this.gameObject, availableActions[0].ActionType);
        }
        else
        {
            // Prompt UI opening and provide actions for population
            playerController.playerUIController.showInteractionUI(this.gameObject, availableActions);
        }
    }
}
