using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Unity.Common.Core.Math;
using Improbable.Math;
using Improbable.Checks;
using Improbable.Player;
using System;

public class PlayerMovementController : MonoBehaviour {

    [Require]
    CheckIsClientSideWriter clientSideCheck;  // Ensures that this runs on the client only

    [Require]
    PlayerMovementStateReader playerMovementStateReader;

    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    public Vector3 targetPosition;
    public Camera myCamera;
    public Vector3 nextStepPosition;

    public GameObject movementMarker;
    public bool bIsMoving;

    public System.Action onStopMovingCallback;

    void OnEnable()
    {
        floorMask = LayerMask.GetMask("Floor");
        targetPosition = transform.position;
        movementMarker = GameObject.Find("MovementMarker_Singleton");
    }

    // Update is called once per frame
    void Update ()
    {
        // Check if there is a click
        if (Input.GetMouseButtonDown(0))
        {
            myCamera = this.GetComponentInChildren<Camera>();

            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = myCamera.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                targetPosition = floorHit.point;
                targetPosition.y = transform.position.y;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                if (this.movementMarker != null)
                {
                    this.movementMarker.transform.position = targetPosition;
                    setMovementMarkerVisible(false);
                }
            }
        }

        if (bIsMoving)
        {
            if (!isMoving())
            {
                Debug.Log("Stopped moving");
                if (onStopMovingCallback != null)
                {
                    onStopMovingCallback();
                }
            }
        }
        bIsMoving = isMoving();
    }

    public Vector3f GetTargetPosition()
    {
        return targetPosition.ToNativeVector3f();
    }

    public bool isMoving()
    {
        return (playerMovementStateReader.TargetPosition.ToUnityVector() - transform.position).magnitude >= 0.1;
    }

    public void setMovementMarkerVisible(bool visible)
    {
        if (this.movementMarker != null)
        {
            this.movementMarker.SetActive(visible);
        }
    }

    public void reset()
    {
        this.targetPosition = this.transform.position;
    }

    public void walkToObject(GameObject targetInteractionObject, Action onReachedInteractionObject)
    {
        Vector3 direction = targetInteractionObject.transform.position - this.transform.position;
        this.targetPosition = targetInteractionObject.transform.position - direction.normalized;
        this.onStopMovingCallback += onReachedInteractionObject;
    }
}
