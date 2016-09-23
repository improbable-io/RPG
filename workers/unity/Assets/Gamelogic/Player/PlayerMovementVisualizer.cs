using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Player;
using Improbable.Corelibrary.Transforms;
using Improbable.Math;
using Improbable.Unity.Common.Core.Math;

public class PlayerMovementVisualizer : MonoBehaviour {

    [Require]
    PlayerMovementStateReader playerMovementState;

    [Require]
    TransformStateWriter transformStateWriter;

    public Vector3 targetPosition;
    public Animator animator;

    void OnEnable()
    {
        playerMovementState.TargetPositionUpdated += OnTargetPositionUpdated;
    }

    private void OnTargetPositionUpdated(Vector3f targetPosition)
    {
        this.targetPosition = targetPosition.ToUnityVector();
    }

    void OnDisable()
    {
        playerMovementState.TargetPositionUpdated -= OnTargetPositionUpdated;
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        float distance = (targetPosition - transform.position).magnitude;

        if (Mathf.Approximately(distance, 0))
        {
            this.transform.position = targetPosition;
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, 10 * Time.deltaTime);            
        }
    }

}
