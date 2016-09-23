using Improbable.Corelibrary.Math;
using UnityEngine;
using Improbable.Unity.Visualizer;
using Improbable.Player;
using Improbable.Corelibrary.Transforms;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;

[EngineType(EnginePlatform.Client)]
public class PlayerClientSideMovementVisualizer : MonoBehaviour {

    [Require]
    PlayerMovementStateReader playerMovementStateReader;

    [Require]
    TransformStateReader transformStateReader;

    public Vector3 targetPosition = Vector3.zero;
    public Vector3 serverPosition = Vector3.zero;

    public float serverSynchronizationDeltaThreshold = 5.0f;
    public float heartbeatTimer = 0f;
    public float heartbeatDelta = 0.5f;

    void OnEnable()
    {
        playerMovementStateReader.TargetPositionUpdated += OnTargetPositionUpdated;
        transformStateReader.LocalPositionUpdated += OnServerPositionUpdated;

        transform.position = Vector3.zero;
    }

    private void OnTargetPositionUpdated(Vector3f targetPosition)
    {
        this.targetPosition = targetPosition.ToUnityVector();
    }

    void OnDisable()
    {
        playerMovementStateReader.TargetPositionUpdated -= OnTargetPositionUpdated;
        transformStateReader.LocalPositionUpdated -= OnServerPositionUpdated;
    }

    private void OnServerPositionUpdated(FixedPointVector3 serverPosition)
    {
        this.serverPosition = serverPosition.ToUnityVector();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 clientPosition = Vector3.MoveTowards(this.transform.position, targetPosition, 10 * Time.deltaTime);
        this.transform.position = Vector3.Lerp(clientPosition, serverPosition, Time.deltaTime);

        if (heartbeatTimer <= 0)
        {
            if ((targetPosition - this.transform.position).magnitude >= serverSynchronizationDeltaThreshold)
            {
                this.transform.position = serverPosition;
            }

            heartbeatTimer = heartbeatDelta;
        }
        else
        {
            heartbeatTimer -= Time.deltaTime;
        }
        
    }
}

