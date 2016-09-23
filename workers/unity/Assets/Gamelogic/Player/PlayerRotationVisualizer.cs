using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Player;
using Improbable.Corelibrary.Transforms;
using Improbable.Math;
using Improbable.Unity.Common.Core.Math;

public class PlayerRotationVisualizer : MonoBehaviour {

    [Require]
    PlayerMovementStateReader playerMovementState;

    public Vector3 targetPosition;
    public GameObject model;

    void OnEnable()
    {
        playerMovementState.TargetPositionUpdated += OnTargetPositionUpdated;
        if (model == null)
        {
            this.model = this.transform.GetChild(0).gameObject;
        }
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
	void Update ()
	{
        float distance = (targetPosition - transform.position).magnitude;

        if (!Mathf.Approximately(distance, 0))
        {
            Vector3 targetPositionAlongEye = targetPosition;
            targetPositionAlongEye.y = model.transform.position.y;
            model.transform.LookAt(targetPositionAlongEye);
        }
       
	}
}
