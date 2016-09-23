using Improbable.Actions;
using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Checks;
using System;

public class PlayerInteractionController : MonoBehaviour {
   
    [Require]
    CheckIsClientSideWriter clientSideCheck;  // Ensures that this runs on the client only

    public Camera myCamera;
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    int isInteractiveMask;              // A layer mask so that a ray can be cast just at gameobjects on the floor layer.

    public Texture2D defaultCursor;
    public Texture2D highlightCursor;
    public CursorMode cursorMode = CursorMode.Auto;

    public GameObject currentHoveredObject;
    public GameObject targetInteractionObject;
    public ActionTypeEnum targetInteraction;
    public PlayerController playerController;

    public InteractionHandler activeHandler;
    public float interactionDuration;
    public float interactionTimer;
    public ActionTypeEnum activeInteraction;

    public GameObject interactionMarker;

    void OnEnable()
    {
        isInteractiveMask = LayerMask.GetMask("IsInteractive");
        this.playerController = this.GetComponent<PlayerController>();
        this.interactionMarker = GameObject.Find("InteractionMarker_Singleton");
    }

    void Update () {
        
        // -- Input

        myCamera = this.GetComponentInChildren<Camera>();
        Ray camRay = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit isInteractiveHit;
        if (Physics.Raycast(camRay, out isInteractiveHit, camRayLength, isInteractiveMask))
        {
            Cursor.SetCursor(highlightCursor, Vector2.zero, cursorMode);
            currentHoveredObject = isInteractiveHit.collider.gameObject.GetEntityObject().UnderlyingGameObject;
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
            currentHoveredObject = null;
        }

        // -- 
        if (activeHandler != null)
        {
            if (interactionTimer <= 0)
            {
                if (activeHandler.isInteractable())
                {
                    activeHandler.onContinuousInteractionStep(playerController, activeInteraction);
                    this.interactionTimer = this.interactionDuration;
                }
                else
                {
                    this.activeHandler = null;
                    this.targetInteractionObject = null;
                }
                
            }
            else
            {
                interactionTimer -= Time.deltaTime;
            }
        }

        this.interactionMarker.SetActive(targetInteractionObject != null);
        if (targetInteractionObject != null)
        {
            this.interactionMarker.transform.position = targetInteractionObject.transform.position;
          
        }
       
    }

    public void cancelContinuousInteraction()
    {
        if (activeHandler != null)
        {
            this.interactionTimer = 0;
            this.activeHandler = null;
            this.targetInteractionObject = null;
        }
    }

    public void interactContinuously(InteractionHandler handler, ActionTypeEnum interactionType, float interactionDuration)
    {
        Debug.Log("interactContinuously(), interactionType=" + interactionType + ", interactionDuration=" + interactionDuration);
        this.activeHandler = handler;
        this.interactionDuration = interactionTimer = interactionDuration;
        this.activeInteraction = interactionType;
    }

    public void reset()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
        currentHoveredObject = null;
    }

    public void setTargetObject(GameObject targetGameObject)
    {
        this.targetInteractionObject = targetGameObject;
    }

    public void setTargetInteraction(ActionTypeEnum interaction)
    {
        this.targetInteraction = interaction;
    }

    public void interact(PlayerController playerController)
    {
        InteractionHandler handler = currentHoveredObject.GetComponent<InteractionHandler>();

        if (handler.isInteractable())
        {
            handler.onPlayerBeginInteraction(playerController);
        }
    }

    public void handleInteraction(PlayerController playerController)
    {
        InteractionHandler handler = targetInteractionObject.GetComponent<InteractionHandler>();
        handler.performInteraction(playerController, targetInteraction);
    }

}
