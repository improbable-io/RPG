using Improbable.Actions;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    public virtual void onPlayerBeginInteraction(PlayerController playerController)
    {

    }

    public virtual void performInteraction(PlayerController playerController, ActionTypeEnum interaction)
    {
        
    }

    public virtual void onContinuousInteractionStep(PlayerController playerController, ActionTypeEnum interaction)
    {
        
    }

    public virtual bool isInteractable()
    {
        return false;
    }
}
