using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Improbable.Unity.Visualizer;
using Improbable;
using Improbable.Player;
using Improbable.Checks;
using Improbable.Actions;
using Improbable.Supplies;

public class PlayerController : MonoBehaviour {

    [Require]
    CheckIsClientSideWriter clientSideCheck;  // Ensures that this runs on the client only

    [Require]
    PlayerMovementStateWriter playerMovementState;

    [Require]
    PlayerActionRequestWriter playerActionRequestWriter;

    [Require]
    PlayerTransactionRequestWriter playerTransactionRequestWriter;

    public PlayerMovementController playerMovementController;
    public PlayerInteractionController playerInteractionController;
    public PlayerAnimationController playerAnimationController;
    public PlayerUIController playerUIController;

	void Update ()
	{
	    if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (playerInteractionController.currentHoveredObject != null)
            {
                playerInteractionController.interact(this);
            }
            else
            {
                // TODO: UI changes based on deselection (clicking the ground) should listen to loss of selection to be triggered?
                playerUIController.hideInteractionUI();
                playerMovementController.setMovementMarkerVisible(playerInteractionController.targetInteractionObject == null);
                playerMovementState.Update.TargetPosition(playerMovementController.GetTargetPosition()).FinishAndSend();

                if (playerInteractionController.activeHandler != null)
                {
                    playerInteractionController.cancelContinuousInteraction();
                }


               
            }
        }

        // Open/Close Shop UI
        if (Input.GetKeyUp(KeyCode.S))
        {
            List<SupplyTypeEnum> productList = new List<SupplyTypeEnum>();
            productList.Add(SupplyTypeEnum.COPPER);
            productList.Add(SupplyTypeEnum.WOOD);
            productList.Add(SupplyTypeEnum.TIN);
            playerUIController.showShopUI(productList);
        }
    }

    public void MoveToAndInteract(GameObject targetInteractionObject, ActionTypeEnum actionType)
    {
        float distance = Vector3.Distance(transform.position, targetInteractionObject.transform.position);

        if (distance > 1)
        {
            // Set interaction properties
            playerInteractionController.setTargetObject(targetInteractionObject);
            playerInteractionController.setTargetInteraction(actionType);

            playerMovementController.walkToObject(targetInteractionObject, onReachedInteractionObject);
            playerUIController.hideInteractionUI();

            playerMovementState.Update.TargetPosition(playerMovementController.GetTargetPosition()).FinishAndSend();
        }
        else
        {
            // Close enough: Interact
            playerInteractionController.handleInteraction(this);
        }
    }

    public void onReachedInteractionObject()
    {
        Debug.Log("OnReachedInteractionObject()");
        playerMovementController.onStopMovingCallback -= onReachedInteractionObject;
        playerInteractionController.handleInteraction(this);
    }

    public void TriggerActionRequest(EntityId targetEntityId, ActionTypeEnum actionType)
    {
        playerActionRequestWriter.Update.TriggerActionRequest(targetEntityId, actionType).FinishAndSend();
    }

    public void TriggerTransactionRequest(SupplyTypeEnum fromSupplyType, SupplyTypeEnum toSupplyType)
    {
        playerTransactionRequestWriter.Update.TriggerTransactionRequest(fromSupplyType, toSupplyType).FinishAndSend();
    }

    public void TriggerActionAnimation(ActionTypeEnum actionType)
    {
        switch (actionType)
        {
            case ActionTypeEnum.HARVEST:
                this.playerAnimationController.triggerAnimationHarvest();
                break;
            default:
                break;
        }
    }
}
