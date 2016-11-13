using UnityEngine;
using System.Collections;
using Improbable.Corelibrary.Transforms;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;

[EngineType(EnginePlatform.Client)]
public class PlayerAnimationVisualizer : MonoBehaviour
{
    public Animator animator;
    public PlayerMovementController playerMovementController;

    [Require]
    public PlayerAnimationReader playerAnimationReader;


    void OnEnable ()
    {
	    if (this.animator == null)
	    {
	        this.animator = this.GetComponentInChildren<Animator>();
	    }
        if (this.playerMovementController == null)
        {
            this.playerMovementController = this.GetComponent<PlayerMovementController>();
        }

        playerAnimationReader.Animation += OnTriggerAnimation;
    }

    void OnDisable()
    {
        playerAnimationReader.Animation -= OnTriggerAnimation;
    }

    public void OnTriggerAnimation(PlayerAnimationPayload playerAnimationPayload)
    {
        if (this.animator != null)
        {
            this.animator.SetTrigger(playerAnimationPayload.animationClipName);
        }
    }

    void Update()
    {
        this.animator.SetBool("Moving", playerMovementController.isMoving());
        this.animator.SetBool("Running", playerMovementController.isMoving());
    }
}
