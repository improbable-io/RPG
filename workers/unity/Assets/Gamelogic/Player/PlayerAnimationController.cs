using UnityEngine;
using System.Collections;
using Improbable.Corelibrary.Transforms;
using Improbable.Player;
using Improbable.Unity.Visualizer;

public class PlayerAnimationController : MonoBehaviour
{

    [Require]
    private PlayerAnimationWriter playerAnimationWriter;

    public void triggerAnimationHarvest()
    {
        playerAnimationWriter.Update.TriggerAnimation("Attack1Trigger").FinishAndSend();
    }

}
